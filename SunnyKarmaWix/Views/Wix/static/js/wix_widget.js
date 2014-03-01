$(function() {

  //set config
  init.config();

  //start listening for actions
  actions.startListening();

});


var init = {

  config: function() {
  }

}

var actions = {

  startListening: function() {
    return actions.submitDonation();
  },

  submitDonation: function() {
    $('.donate input[type="submit"]').click(function() {
      param = $('.donate input[name="amount"]').val();

      if (validation.submitDonation(param)) {
        url = 'wix_signup.html?amount=' + param;
        document.location.href = url;
      }

      return false;
    });
  }

};



var responses = {



};



var validation = {

  submitDonation: function(param) {
    if ((param) && ($.isNumeric(param))) {
      return true
    } else {
      return false
    }
  }

};