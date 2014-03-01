using System;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class DataManager
    {
        private static CloudTableClient GetCloudTableClient()
        {
            return CloudStorageAccount.Parse("").CreateCloudTableClient();
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection("");
        }

        public static void AddWixOwner(WixOwner owner)
        {
            CloudTableClient client = GetCloudTableClient();
            CloudTable wixOwnerTable = client.GetTableReference("WixOwner");
            wixOwnerTable.Execute(TableOperation.Insert(owner));
        }

        public static void DonateToCause(string username, string causeId, int amount)
        {
            UpdateBalance(GetUserId(username), -amount);
            UpdateBalance(causeId, amount);
            AddNativeBalanceChange(causeId, amount, "DonationOfUser", username);

            // update cause cash
            var client = GetCloudTableClient();
            var table = client.GetTableReference("Cause");
            var cause = table.ExecuteQuery(table.CreateQuery<Cause>()).First(x => x.CauseID == causeId);
            cause.Cash += amount;
            table.Execute(TableOperation.Replace(cause));

            // update spent money
            table = client.GetTableReference("Profile");
            var profile = table.ExecuteQuery(table.CreateQuery<Profile>()).First(x => x.Username == username);
            profile.SpentMoney += amount;
            table.Execute(TableOperation.Replace(profile));
        }

        private static string GetUserId(string username)
        {
            CloudTableClient client = GetCloudTableClient(); 
            CloudTable userTable = client.GetTableReference("User");
            var user = userTable.ExecuteQuery(userTable.CreateQuery<User>()).FirstOrDefault(x => x.Username == username);
            return user != null ? user.UserID : null;
        }

        private static void UpdateBalance(string balanceId, int balanceChange)
        {
            using (SqlConnection connection = GetConnection())
            {
                string QueryString = @"UPDATE Balance SET AccountBalance = AccountBalance + @AccountBalanceChange, 
                                        AvailableBalance = AvailableBalance + @AvailableBalanceChange, Changed = @Changed 
                                        WHERE BalanceID = @BalanceID 
                                        IF @@ROWCOUNT=0
                                        INSERT INTO Balance(BalanceID,  AccountBalance, AvailableBalance, Changed) 
                                        VALUES(@BalanceID, @AccountBalanceChange, @AccountBalanceChange, @Changed)";

                var cmd = new SqlCommand(QueryString, connection);
                cmd.Parameters.AddWithValue("@AccountBalanceChange", balanceChange);
                cmd.Parameters.AddWithValue("@AvailableBalanceChange", balanceChange);
                cmd.Parameters.AddWithValue("@Changed", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@BalanceID", balanceId);

                cmd.ExecuteNonQuery();
            }
        }

        private static void AddNativeBalanceChange(string balanceId, int change, string descriptionType,
                                                   string descriptionValue)
        {
            var client = GetCloudTableClient();
            var table = client.GetTableReference("NativeBalanceChanges");

            table.Execute(TableOperation.Insert(new NativeBalanceChange
                {
                    BalanceID = balanceId,
                    Change = change,
                    Description = descriptionType,
                    DescriptionValue = descriptionValue
                }));
        }

        public static void AwardKarmaPointsToUser(string username, string causeId, int donationAmount)
        {
            AddNativeBalanceChange(GetUserId(username), donationAmount, "DonationToTheCause", causeId);
        }

        public static void AwardKarmaPointsToOwner(string username, string causeId, int donationAmount)
        {
            //            People.UpdatePerson(UserID, RaisedMoney: DonateValue);
            AddNativeBalanceChange(GetUserId(username), donationAmount, "AddForCause", causeId);
        }
    }
}