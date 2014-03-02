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
    return actions.submitDonation(),
           actions.submitSignin(),
           actions.submitSignup(),
           actions.pressSkip(),
           actions.pressMenu()
  },

  submitDonation: function() {
    $('.donate input[type="submit"]').click(function() {
      param = $('.donate input[name="amount"]').val();

      if (validation.submitDonation(param)) {
        url = 'wix_signin_signup.html?amount=' + param;
        document.location.href = url;
      }

      return false;
    });
  },

  submitSignin: function() {
    $('.signin input[type="submit"]').click(function() {
      email = $('.signin input[name="email"]').val();
      password = $('.signin input[name="password"]').val();
      amount = helper.getURLParameter('amount');

      /* Old params sending
      if (validation.submitSignin(email, password, amount)) {
        url = 'logging.html?amount=' + amount + '&email=' + email + '&password=' + password;
        document.location.href = url;
      }
      */

      if (validation.submitSignin(email, password, amount)) {
        $.get("api/signin.json", {
          email: email,
          password: password
        }).done(function(data) {
          (data.Status == 'ok') ? alert('Login Successful') : alert('Login Unsuccessful');;
        });
      }

      return false;
    });
  },

  submitSignup: function() {
    $('.signup input[type="submit"]').click(function() {
      username = $('.signup input[name="username"]').val();
      email = $('.signup input[name="email"]').val();
      zipcode = $('.signup input[name="zipcode"]').val();
      password = $('.signup input[name="password"]').val();
      amount = helper.getURLParameter('amount');

      /* Old params sending
      if (validation.submitSignup(username, email, zipcode, password, amount)) {
        url = 'signup.html?amount=' + amount + '&username=' + username + '&email=' + email + '&zipcode=' + zipcode + '&password=' + password;
        document.location.href = url;
      }
      */

      if (validation.submitSignup(username, email, zipcode, password, amount)) {
        $.get("api/signup.json", {
          email: email,
          password: password,
          username: username,
          zipcode: zipcode
        }).done(function(data) {
          (data.Status == 'ok') ? alert('Signup '+ data.Username) : alert('Try again');
        });
      }

      return false;
    });
  },

  submitDonate: function() {
    $('.donate input[type="submit"]').click(function() {
      username = $('.signup input[name="username"]').val();
      ccnumber = $('.signup input[name="ccnumber"]').val();
      ccsecurity = $('.signup input[name="ccsecurity"]').val();
      ccexpdate = $('.signup input[name="ccexpdate"]').val();
      wixinstanceid = $('.signup input[name="wixinstanceid"]').val();
      amount = helper.getURLParameter('amount');

      /* Old params sending
      if (validation.submitSignup(username, email, zipcode, password, amount)) {
        url = 'signup.html?amount=' + amount + '&username=' + username + '&email=' + email + '&zipcode=' + zipcode + '&password=' + password;
        document.location.href = url;
      }
      */
 
      if (validation.submitDonate(username, ccnumber, ccsecurity, ccexpdate, amount, wixinstanceid)) {
        $.get("api/donate.json", {
        username: username,
        ccnumber: ccnumber,
        ccsecurity: ccsecurity,
        ccexpdate: ccexpdate,
        amount: amount,
        wixinstanceid: wixinstanceid
        }).done(function(data) {
          (data.Status == 'ok') ? alert('Donation done') : alert('Try again');
        });
      }

      return false;
    });
  },

  pressSkip: function() {
    $('.signin-signup .skip').click(function() {
      amount = helper.getURLParameter('amount');
      no_signin_signup = 1;

      if (validation.pressSkip(amount)) {
        url = 'donate.html?amount=' + amount + '&no_signin_signup=' + no_signin_signup;
        document.location.href = url;
      }

      if (validation.pressSkip(amount)) {
      }

      return false;
    });
  },

  pressMenu: function() {
    $('.menu a').click(function() {

      if (!$(this).hasClass('active')) {

        $('.menu a').each(function() {
          $(this).removeClass('active');
        });

        elClass = '.' + $(this).attr('class');

        $(this).addClass('active');

        $('.signin-signup div').each(function() {
          $(this).removeClass('active');
        });

        $(elClass + '_box').addClass('active');
      }

      return false;
    });
  }

};




var validation = {

  submitDonation: function(param) {
    if ((param) && ($.isNumeric(param))) {
      return true
    } else {
      return false
    }
  },

  submitSignin: function(email, password, amount) {
    if ((email) && (password) && (amount)) {
      return true
    } else {
      return false
    }
  },

  submitSignup: function(username, email, zipcode, password, amount) {
    if ((username) && (email) && (zipcode) && (password) && (amount)) {
      return true
    } else {
      return false
    }
  },

  submitDonate: function(username, ccnumber, ccsecurity, ccexpdate, amount, wixinstanceid) {
    if ((username) && (ccnumber) && (ccsecurity) && (ccexpdate) && (amount) && (wixinstanceid)) {
      return true
    } else {
      return false
    }
  },

  pressSkip: function(amount) {
    if (amount) {
      return true
    } else {
      return false
    }
  }

};



var helper = {

  getURLParameter: function(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');

    for (var i = 0; i < sURLVariables.length; i++) {
      var sParameterName = sURLVariables[i].split('=');

      if (sParameterName[0] == sParam) {
        return sParameterName[1];
      }
    }
  }

}