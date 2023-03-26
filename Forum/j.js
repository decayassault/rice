"use strict"
var xmlhttp = new XMLHttpRequest();
var doc = window.document;
var content = "content";
var True = true;
var four = 4;
var time = 10;
var twozz = 200;
var thzz = 300;
var captchaRefreshInterval = 100000;
var awaitRefreshInterval = 10000;
var xhrTimeout = 30000;
var z = 0;
var o = 1;
var mo = -1;
var intervalId;
var updateInterval;
var pageToUpdate;
var get = "GET";
var post = "POST";
var cache = {
};
var sites = ["/maincontent", "/registrationpage","/unknown","/loginpage"];

var nickEr = "Недопустимый ник. Ник может содержать от 4 до 25 знаков, не должен начинаться или оканчиваться пробелом, может содержать пробелы, русские буквы обоих регистров, цифры.";
var emailEr = "Недопустимый адрес электронной почты. Действующий почтовый ящик необходим для связи с администрацией.";
var pwdEr = "Недопустимый пароль. Пароль может содержать русские буквы, знаки пунктуации, цифры и иметь длину от 8 до 50 знаков.";
var loginEr = "Недопустимый логин. Логин может иметь длину от 6 до 25 знаков, содержать русские буквы, цифры.";
var captchaEr = "Неверно введено число с картинки.";
var existEr = "Недопустимые учётные данные. Попробуйте изменить ник, логин, пароль.";
var replyEr = "Недопустимый текст сообщения.";
var updateMsg = "Страница перезагрузится через 10 секунд.";

var rus = ['а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж',
          'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х',
          'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', ];
var spec = ['.', ',', '-', ' ', '!', '?', ';', ':', '"'];
var num = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];


function reload() {
    var len = sites.length;
    var i;
    for (i = z; i < len; i++) {
        var url = sites[i];
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (isOkXhr(xhr))
                cache[url] = xhr.responseText;
        }
        xhrStart(url, xhr);
    }
}
reload();
//setInterval(reload, 10000);

function xhrStart(url, xhr) {
    if (xhr == undefined) {
        xmlhttp.open(get, url, True);
        xmlhttp.timeout = xhrTimeout;
        xmlhttp.send();
    }
    else {
        xhr.open(get, url, True);
        xhr.timeout = xhrTimeout;
        xhr.send();
    }
}

function filterNonRussian(text) {
    var result = '';
    var rusCount = 0;
    var specCount = 0;
    var sym = '';
    var l = text.length;

    for (var i = 0; i < l; i++) {
        sym = text[i];
        if (rus.indexOf(sym.toLowerCase()) !== mo)
            result += sym,
                rusCount++;
        else if ((spec.indexOf(sym) !== mo) || (num.indexOf(sym) != mo))
            result += sym,
                specCount++;
    }
    if ((rusCount / l < 0.5) || (rusCount / specCount < 0.8))
        result = '';

    return result;
}

function checkpassword(pwd, pwdconfirm) {
    var result;

    if (pwd === pwdconfirm) {
        var len = pwd.length;
        var flag = (len >= 8) && (len <= 50);
        if (flag) {
            var flagrus = false;
            var flagspec = false;
            var flagnum = false;
            for (var i = z, l = pwd.length; i < l; i++) {
                if ((rus.indexOf(pwd[i].toLowerCase()) !== mo) || (rus.indexOf(pwd[i]) !== mo))
                    flagrus = true;
                else if (spec.indexOf(pwd[i]) !== mo)
                    flagspec = true;
                else if (num.indexOf(pwd[i]) !== mo)
                    flagnum = true;
            }
            if (flagrus && flagspec && flagnum)
                result = true;
            else result = false;
        }
        else result = false;
    }
    else result = false;

    return result;
}

function checklogin(login) {
    var result = false;

    var len = login.length;
    if ((len >= 6) && (len <= 25)) {
        for (var i = z; i < len; i++) {
            if (!((rus.indexOf(login[i].toLowerCase()) != mo)
                || (num.indexOf(login[i]) != mo))) {
                break;
            }
            else result = true;
        }
    }

    return result;
}

function checkemail(email, emailconfirm) {
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

function checknick(nick) {
    var result = false;
    var space = ' ';
    var leng = nick.length;
    if ((leng >= four) && (leng <= 25)
            && (nick[z] != space) && (nick[leng - 1] != space)) {
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
        if (letflag)
            result = true;
        else result = false;
    }

    return result;
}

function checkcaptcha(captcha) {
    var result = false;

    var len = captcha.length;
    if (len < 20)
        for (var i = z; i < len; i++) {
            if (num.indexOf(captcha[i]) != mo)
                result = true;
            else {
                break;
            }
        }

    return result;
}

function getExpired(min) {
    var date = new Date();
    return new Date(date.getFullYear(),
                      date.getMonth(),
                date.getDate(), date.getHours(),
                date.getMinutes() + min);
}

function isOkXhr(xhr) {
    if (xhr !== undefined) {
        (xhr.readyState == four &&
        (xhr.status >= twozz && xhr.status < thzz));
    }
    else
        return (xmlhttp.readyState == four &&
            (xmlhttp.status >= twozz && xmlhttp.status < thzz));
}

function g(url)
{
    n(url);
    /*cache disabled
    if (cache[url] == undefined) {
        cache[url] = [];        
        a(url);
    }
    else if (cache[url][o] < new Date())         
        a(url);    
    else
        doc.getElementById(content).innerHTML = cache[url][z];*/
}

function u()
{   
    m('/login/', 'a');
}

function s() {
    var id = doc.getElementsByClassName('s')[z].innerHTML;
    var result;
    var obj = doc.getElementById('text').value;
    var len = obj.length;
    obj = filterNonRussian(obj);
    if ((len > 1000) || (len < 10))
        result = false;
    else result = true;
    if (result)
    {
        var url = '/reply/' + id + '?t=' + obj;
        xmlhttp.onreadystatechange = function () {
            if (isOkXhr()) {
                correctCacheAnswer(id);
            }
        }
        xhrStart(url);
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

function t(url, container) {
    m(url, container);
    /*cache disabled
    if (cache[url] == undefined) {
        cache[url] = [];
        r(url,container);
    }
    else if (cache[url][o] < new Date())
        r(url,container);
    else
        doc.getElementById(container).innerHTML = cache[url][z];*/
}

function a(url) {
    xmlhttp.onreadystatechange = function () {
        if (isOkXhr()) {
            doc.getElementById(content).innerHTML = xmlhttp.responseText;
            cache[url][z] = xmlhttp.responseText;
            var date = new Date();
            cache[url][o] =
                new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes() + 5);
        }
    }
    xhrStart(url);
}
function r(url, target) {
    xmlhttp.onreadystatechange = function () {
        if (isOkXhr()) {
            doc.getElementById(target).innerHTML = xmlhttp.responseText;
            cache[url][z] = xmlhttp.responseText;
            cache[url][o] = getExpired(5);
        }
    }
    xhrStart(url);
}

function m(url, target) {
    xmlhttp.onreadystatechange = function () {
        if (isOkXhr()) {
            doc.getElementById(target).innerHTML = xmlhttp.responseText;
                    }
    }
    xhrStart(url);
}

function register(captcha, login, password, email, nick)
{
    var url = "/register/";
    var response;
    xmlhttp.onreadystatechange = function () {
        if (isOkXhr()) {
            n('/loginpage/');
        }
    }

    xmlhttp.open(post, url, True);    
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send("captcha="+captcha+"&login="+login+"&password="+password+
        "&email="+email+"&nick="+nick);
}

function p() {
    clearInterval(intervalId);
    clearInterval(updateInterval);
    g('/maincontent/');
}

function c() {
    clearInterval(intervalId);
    var result;  
    var msg = 'msg';

    var captcha = doc.getElementById('captcha').value;
    var nick = doc.getElementById('nick').value;
    var email = doc.getElementById('email').value;
    var emailconfirm = doc.getElementById('emailconfirm').value;
    var login = doc.getElementById ('login').value;
    var pwd = doc.getElementById('password').value;
    var pwdconfirm = doc.getElementById('pwdconfirm').value;

    result = checkpassword (pwd, pwdconfirm);
    
    if (!result)
        doc.getElementById(msg).innerHTML = pwdEr;
    else {
        result = checklogin (login);

        if (!result)
            doc.getElementById(msg).innerHTML = loginEr;
        else {
            result = checkemail(email, emailconfirm);

            if (!result)
                doc.getElementById(msg).innerHTML = emailEr;
            else {
                result = checknick(nick);

                if(!result)
                    doc.getElementById(msg).innerHTML = nickEr;
                else 
                     {
                            result = checkcaptcha(captcha);
                            
                            if(!result)
                                doc.getElementById(msg).innerHTML = captchaEr;
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
        if (isOkXhr()) {
            doc.getElementById(content).innerHTML = xmlhttp.responseText;
        }
    }
    xhrStart(url);
}

function auth(captcha, login, password)
{
    var url = "/authenticate/";
    xmlhttp.onreadystatechange = function () {
        if (isOkXhr()) {
            g('/maincontent/');
        }
    }

    xmlhttp.open(post, url, True);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xmlhttp.send("captcha=" + captcha + "&login=" + login + "&password=" + password);
}

function j()
{
    pageToUpdate = '/loginpage/';
    n(pageToUpdate);
    intervalId = setInterval('setTime();', captchaRefreshInterval);
}

function h()
{    
    pageToUpdate = '/registrationpage/';
    n(pageToUpdate);
    intervalId = setInterval('setTime();', captchaRefreshInterval);
}

function setTime()
{
    clearInterval(intervalId);
    doc.getElementById('msg').innerHTML
        = updateMsg;
    updateInterval = setInterval('updatePage();', awaitRefreshInterval);
}

function updatePage()
{
    n(pageToUpdate);
    clearInterval(updateInterval);
    intervalId = setInterval('setTime();',captchaRefreshInterval);
}

function d() {
    clearInterval(intervalId);
    var result;
    var msg = 'msg';

    var captcha = doc.getElementById('captcha').value;
    var login = doc.getElementById('login').value;
    var password = doc.getElementById('password').value;

    result = checkpassword (password,password);
    
    if (!result)
        doc.getElementById(msg).innerHTML = pwdEr;
    else {
        result = checklogin(login);
        if (!result)
            doc.getElementById(msg).innerHTML = loginEr;
        else {
            result = checkcaptcha(captcha);

            if (!result)
                doc.getElementById(msg).innerHTML = captchaEr;
            else {                
                auth(captcha, login, password);
            }
        }
    }
}

function newTopic()
{
    m('/newtopic/', 'topic');
}

function startTopic()
{
    var title = doc.getElementsByTagName('input')[z].value;
    var id = doc.getElementsByClassName('s')[z].innerHTML;
    var result;
    var msg = doc.getElementById('text').value;
    var msglen = msg.length;
    var titlelen=title.length;
    title = filterNonRussian(title);
    msg = filterNonRussian(msg);
    if (((msglen > 1000) || (msglen < 10))
        || ((titlelen > 100) || (titlelen < 5)))
        result = false;
    else result = true;
    if (result) {
        var url = '/starttopic/' + id + '?t=' + title+'&m='+msg;
        xmlhttp.onreadystatechange = function () {
            if (isOkXhr()) {
                n('/maincontent/');
                //correctCacheAnswer(title);
            }
        }
        xhrStart(url);
    }
    //TODO
}
function openDialogsList()
{
    n('/dialog/1');
}
