if('serviceWorker' in navigator) {
    navigator.serviceWorker
        .register('/sw.js')
        .then(function() {
        })
        .catch(function() {
        });
}

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

window.fadeIn = function (obj) {
    $(obj).fadeIn(1000);
}

function changeTheme() {
    $('body').toggleClass('dark');
    $('.sidebar').toggleClass('darkSidebar');
    $(".btn-sidebar").toggleClass('darkButton');
    $(".btn-download").toggleClass('darkButton');
    $(".toggler").toggleClass('darkToggle fa-sun fa-moon')
    $(".options").toggleClass('darkOptions');
    $("#languageTitle").toggleClass('darkLanguage');
    $("#cb1").toggleClass('tgl-dark');
    $("#cb2").toggleClass('tgl-dark');
    $("#themelanguage").toggleClass('darkThemeLanguage');
}

function changeLanguage(lang) {
    $(".imgFlag").attr("src", "/images/" + lang + ".svg");
}

var orientation = false;
var gameToken = null;

$(document).ready(function () {
    //console.log(document.getElementById('GameToken').value);
    if($(".game-canvas").height() > $(".game-canvas").width()){
        $(".game-canvas").addClass("portrait");
        $(".game-canvas").removeClass("landscape");
        orientation = false;
    }
    else{
        $(".game-canvas").addClass("landscape");
        $(".game-canvas").removeClass("portrait");
        orientation = true;
    }
    
    $(window).resize(function () {
        if($(".game-canvas").height() > $(".game-canvas").width() && orientation){
            $(".game-canvas").addClass("portrait");
            $(".game-canvas").removeClass("landscape");
            orientation = !orientation;
        }
        else if ($(".game-canvas").height() < $(".game-canvas").width() && !orientation){
            $(".game-canvas").addClass("landscape");
            $(".game-canvas").removeClass("portrait");
            orientation = !orientation;
        }
    });
    
    var theme = getCookie("theme");
    var language = getCookie("language");

    if (theme == "dark") {
        changeTheme();
    }

    changeLanguage(language);

    $('#cb1').prop('checked', false);
    $('#cb2').prop('checked', false);

    // Toggle dark theme
    $('.theme-toggle').click(function () {
        changeTheme();

        if (theme === "dark") {
            theme = "light";
        } else {
            theme = "dark";
        }

        document.cookie = "theme=" + theme;
    });

    // Language button pressed
    $('.languageSwitch').click(function () {
        if (language == "dutch") {
            language = "english";
        } else {
            language = "dutch";
        }

        document.cookie = "language=" + language;
        if (gameToken != null){
            window.location.href = "/Game/" + language;
        }
        else if (window.location.href.toString().charAt(window.location.href.toString().length - 1 ) == "/"){
            window.location.href = window.location.href + "home/" + language;
        }
        else if (window.location.pathname.toString().startsWith("/games")){
            window.location.href = window.location.href + "/home/" + language;
        }
        else{
            window.location.href = window.location.href + "/" + language;
        }
    });
});