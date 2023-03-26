"use strict"
var xmlhttp;
try {
    xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
} catch (e) {
    try {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    } catch (E) {
        xmlhttp = false;
    }
}
if (!xmlhttp && typeof XMLHttpRequest != undefined) {
    xmlhttp = new XMLHttpRequest();
}
var content = "content";
var True = true;
var four = 4;
var twozz = 200;
var z = 0;
var o = 1;
var mo = -1;
var get = "GET";
var cache = {
};

var nickEr = "Недопустимый ник. Ник может содержать от 4 до 25 знаков, не должен начинаться или оканчиваться пробелом, может содержать пробелы, русские буквы обоих регистров, цифры.";
var emailEr = "Недопустимый адрес электронной почты. Действующий почтовый ящик необходим для связи с администрацией.";
var pwdEr = "Недопустимый пароль. Пароль может содержать русские буквы, знаки пунктуации, цифры и иметь длину от 8 до 50 знаков.";
var loginEr = "Недопустимый логин. Логин может иметь длину от 6 до 25 знаков, содержать русские буквы, цифры.";
var captchaEr = "Неверно введено число с картинки.";
var existEr = "Недопустимые учётные данные. Попробуйте изменить ник, логин, пароль.";
var replyEr = "Недопустимый текст сообщения.";

var rus = ['а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж',
          'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х',
          'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', ];
var spec = ['.', ',', '-', ' ', '!', '?', ';', ':', '"'];
var num = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

function g(url)
{
    if (cache[url] == undefined) {
        cache[url] = [];
        a(url);
    }
    else if (cache[url][o] < new Date())
        a(url);
    else
        document.getElementById(content).innerHTML = cache[url][z];
}

function u()
{   
    m('/login/', 'a');
}

function s() {
    var id = document.getElementsByClassName('s')[z].innerHTML;
    var result;
    var obj = document.getElementById('r').value;
    obj = filterNonRussian(obj);
    if ((obj.length > 1000) || (obj.length < 10))
        result = false;
    else result = true;

    if (result)
    {
        var url = '/reply/' + id + '?t=' + obj;
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
                correctCacheAnswer(id);
            }
        }
        xmlhttp.open(get, url, True);
        xmlhttp.send();
    }
}

function correctCacheAnswer(id, response)
{
        n('/maincontent/');
        id++;
        var name = "/thread/" + id + "?";
        var pages = [];
        for(var prop in cache)
        {
            if (prop.indexOf(name) != mo) {
                var link = prop;
                var temp = link.split('=');                
                pages.push(temp[o]);                
            }
        }
        var max = o;
        for (var i = z, len = pages.length; i < len; i++)
            if (pages[i] > max)
                max = pages[i];
        name = name + "page=" + max;
        cache[name] = undefined;
}

function filterNonRussian(text)
    {            
        var result = '';
        var rusCount = 0;
        var specCount = 0;
        var sym = '';
        var l = text.length;

        for(var i = 0; i < l; i ++)
        {
            sym = text[i];
            if (rus.indexOf(sym.toLowerCase()) !== mo)
                result += sym,
                    rusCount++;
            else if ((spec.indexOf(sym) !== mo)||(num.indexOf(sym)!=mo))
                result += sym,
                    specCount++;
        }
        if ((rusCount / l < 0.5) || (rusCount / specCount < 0.8))
            result = '';

        return result;
}

function t(url, container) {
    if (cache[url] == undefined) {
        cache[url] = [];
        r(url,container);
    }
    else if (cache[url][o] < new Date())
        r(url,container);
    else
        document.getElementById(container).innerHTML = cache[url][z];
}

function a(url) {
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            document.getElementById(content).innerHTML = xmlhttp.responseText;
            cache[url][z] = xmlhttp.responseText;
            var date = new Date();
            cache[url][o] =
                new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes() + 5);
        }
    }
    xmlhttp.open(get, url, True);
   
    xmlhttp.send();
}
function r(url, target) {
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            document.getElementById(target).innerHTML = xmlhttp.responseText;
            cache[url][z] = xmlhttp.responseText;
            cache[url][o] = getExpired(5);
        }
    }
    xmlhttp.open(get, url, True);

    xmlhttp.send();
}

function m(url, target) {
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            document.getElementById(target).innerHTML = xmlhttp.responseText;
                    }
    }
    xmlhttp.open(get, url, True);
   
    xmlhttp.send();
}

function checkpassword (pwd, pwdconfirm)
{    
    var result;

    if (pwd === pwdconfirm) {
        var len = pwd.length;
        var flag = (len >= 8) && (len <= 50);
        if (flag) {
            var flagrus = false;            
            var flagspec = false;
            var flagnum = false;
            for (var i = z, l = pwd.length; i < l; i++)
            {                
                if ((rus.indexOf(pwd[i].toLowerCase()) !== mo)||(rus.indexOf(pwd[i])!==mo)) 
                    flagrus = true;                
                else if (spec.indexOf(pwd[i]) !== mo) 
                    flagspec = true;
                else if (num.indexOf(pwd[i]) !== mo) 
                    flagnum = true;
            }
            if (flag&&flagrus && flagspec && flagnum)
                result = true;
            else result = false;
        }
        else result = false;
    }
    else result = false;

    return result;
}

function checklogin (login)
{
    var result;
    
    var len = login.length;
    if ((len >= 6) && (len <= 25)) {
        for(var i = z; i<len; i ++)
        {
            if (!((rus.indexOf(login [i].toLowerCase ()) != mo)
                || (num.indexOf(login [i]) != mo)))
            {
                result = false;
                break;
            }
            else result = true;
        }
    }
    else result = false;

    return result;
}

function checkemail(email, emailconfirm)
{
    var result;
    var dogflag = false;
    var dotflag = false;
    var equflag = false;
    
    if (email === emailconfirm) {
        equflag = true;
        var index = email.indexOf('@');
        if (index !== mo) {
            dogflag = true;                    
            if (index > email.split("").reverse().indexOf('.'))
                dotflag = true;
        }
    }
    if (equflag && dogflag && dotflag)
        result = true;
    else result = false;

    return result;
}

function checknick(nick)
{
    var result = false;
    var space = ' ';
    var leng = nick.length;
    if ((leng >= four) && (leng <= 25)
            && (nick[z] != space)&&(nick[leng - 1] != space)) {
        var letflag = false;
        nick = nick.split(space).join("");
        leng = nick.length;
        if ((leng >= o) && (leng <= 22))
            for (var i = z; i < leng; i++)
                if ((rus.indexOf(nick[i]) !== mo)
                    || (rus.indexOf(nick[i].toLowerCase()) != mo)
                    || (num.indexOf(nick[i]) != mo))
                    letflag = true;
                else {
                    letflag = false;
                    break;
                }
        else letflag = false;
        if(letflag)
            result= true;
        else result= false;
    }

    return result;
}

function checkcaptcha(captcha)
{
    var result;
    
    var len = captcha.length;
    if (len < 20)
        for(var i = z; i < len; i ++)
        {
            if(num.indexOf(captcha [i]) != mo)
                result =true;
            else {
                result = false;
                break;
            }
        }
    else result = false;

    return result;
}

function register(captcha, login, password, email, nick)
{
    var url = "/register/";
    var response;
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            n('/LoginPage/');
        }
    }
    xmlhttp.open("POST", url, True);    
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send("captcha="+captcha+"&login="+login+"&password="+password+
        "&email="+email+"&nick="+nick);
}

function c() {
    var result;  
    var msg = 'msg';

    var captcha = document.getElementById('captcha').value;
    var nick = document.getElementById('nick').value;
    var email = document.getElementById('email').value;
    var emailconfirm = document.getElementById('emailconfirm').value;
    var login = document.getElementById ('login').value;
    var pwd = document.getElementById('password').value;
    var pwdconfirm = document.getElementById('pwdconfirm').value;

    result = checkpassword (pwd, pwdconfirm);
    
    if (!result)
        document.getElementById(msg).innerHTML = pwdEr;
    else {
        result = checklogin (login);

        if (!result)
            document.getElementById(msg).innerHTML = loginEr;
        else {
            result = checkemail(email, emailconfirm);

            if (!result)
                document.getElementById(msg).innerHTML = emailEr;
            else {
                result = checknick(nick);

                if(!result)
                    document.getElementById(msg).innerHTML = nickEr;
                else 
                     {
                            result = checkcaptcha(captcha);
                            
                            if(!result)
                                document.getElementById(msg).innerHTML = captchaEr;
                            else
                            {
                                register(captcha,login, pwd, email, nick);   
                            }
                        }
                }                
            }
        }
    }       

function n(url) {
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            document.getElementById(content).innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.open(get, url, True);
    xmlhttp.send();
}

function auth(captcha, login, password)
{
    var url = "/authenticate/";
    var response;
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == four && xmlhttp.status == twozz) {
            g('/maincontent/');
        }
    }
    xmlhttp.open("POST", url, True);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send("captcha=" + captcha + "&login=" + login + "&password=" + password);
}

function getExpired (min)
{
    var date = new Date();
    return new Date(date.getFullYear(),
                      date.getMonth(), date.getDate(), date.getHours(), date.getMinutes() + min);
}

function j()
{
    n('/LoginPage/');
}

function h()
{
    n('/RegistrationPage/');
}

function d()
{
    var result;
    var msg = 'msg';

    var captcha = document.getElementById('captcha').value;
    var login = document.getElementById('login').value;
    var password = document.getElementById('password').value;

    result = checkpassword (password,password);
    
    if (!result)
        document.getElementById(msg).innerHTML = pwdEr;
    else {
        result = checklogin(login);
        if (!result)
            document.getElementById(msg).innerHTML = loginEr;
        else {
            result = checkcaptcha(captcha);

            if (!result)
                document.getElementById(msg).innerHTML = captchaEr;
            else {                
                auth(captcha, login, password);
            }
        }
    }
}

