var register = false;
var login = false;

$(document).ready(function () {
    $("#btnSignIn").click(function(){
        if ($(".container").hasClass("toggle")){
            $(".container").removeClass("toggle");
            $("#btnSignIn").addClass("selected");
            $("#btnRegister").removeClass("selected");
            document.getElementById('register').reset();
        }
    });
    $("#btnRegister").click(function(){
        if (!$(".container").hasClass("toggle")){
            $(".container").addClass("toggle");
            $("#btnRegister").addClass("selected");
            $("#btnSignIn").removeClass("selected");
            document.getElementById('login').reset();
        }
    });
    if (register){
        $(".container").addClass("toggle");
        $("#btnRegister").addClass("selected");
        $("#btnSignIn").removeClass("selected");
    }
    if (login){
        $(".container").removeClass("toggle");
        $("#btnSignIn").addClass("selected");
        $("#btnRegister").removeClass("selected");
    }
})