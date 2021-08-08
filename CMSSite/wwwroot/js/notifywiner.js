"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
let mark = false;
connection.start().then(function () {
    //console.log('start');
    
    let data = $('#dateToarst').val().toString();
    let hourse = data.split(':')[0];
    let minu = data.split(':')[1];
    let minuMore  = +minu + 5;
    let second = data.split(':')[2];
    //console.log(hourse, minu);
    let timer = setInterval(function () {
        let dj = new Date();
        //console.log(mark);
        //console.log(minu);
        if (mark === false && dj.getHours().toString() === hourse && dj.getMinutes() >= +minu && dj.getMinutes() <= +minuMore) {
            //clearInterval(timer);
            connection.invoke("TopOfYesterday").catch(function (err) {
                return console.error(err.toString());
            });
        } else {
        }
    }, 1000);
}).catch(function (err) {
    return console.error(err.toString());
});


connection.on("ReceiveWinner", function (name, avatar, flag, departmentName) {
    console.log(name);
    if (name != "") {
        $("#pop_up-1st").addClass("show");
        $('#pop_up-1st .name').text(`${name}`);
        $('#pop_up-1st .address').text(`${departmentName}`);
        $('#pop_up-1st .avt-container').html(`
            <img src="/Uploads/Files/${avatar == "" ? "no_photo_avatar.png" : name}" alt="avatar" />
        `);
    }
   
    mark = flag;
});
