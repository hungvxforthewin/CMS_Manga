"use strict";
//HungVX CHAT REAL TIME
$(document).ready(function () {
    $('.isNumber').keyup(delay(function (e) {
        let v = $(this).val();
        v = v.replace(/[^0-9]+/g, '');
        $(this).val(commaSeparateNumber(v));

    }, 0));
    // success
    //$('#btnSuccess').trigger('click');
    //$('#btnChat').attr('disabled', 'disabled');    
    // end
    let initMonney = $('#initMonney').attr('data-monney');
    let minMonney = $('#minMonney').attr('data-monney');
    let maxMonney = $('#maxMonney').attr('data-monney');
    $('#initMonney').html(commaSeparateNumber(initMonney) + ' VND');
    $('#minMonney').html(commaSeparateNumber(minMonney) + ' VND');
    $('#maxMonney').html(commaSeparateNumber(maxMonney) + ' VND');
    // HungVX COUNTDOWN
    let dest = new Date($('#dataCountDown').attr('data-countdown')).getTime();
    let timer = setInterval(function () {

        let now = new Date().getTime();
        let deff = dest - now;
        let displayCountDown = `${0} Ngày ${0} giờ ${0} phút ${0} giây`;
        if (deff < 0) {     
            $('#btnChat').attr('disabled', 'disabled');
            $('#btnChat').html('Hết thời gian đấu giá');
            $('#joinBonus').hide();
            clearInterval(timer);   
            $('#countDown').html(displayCountDown);
            let objectCountDown = {
                DisplayCountDown: displayCountDown,
                ProductId: $('#ProductId').val()
            }
            connection.invoke("SendCountDown", objectCountDown);
        } else {

            let day = Math.floor(deff / (1000 * 60 * 60 * 24));
            let hour = Math.floor( (deff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60) );
            let minutes = Math.floor( (deff % (1000 * 60 * 60)) / (1000 * 60) );
            let second = Math.floor((deff % (1000 * 60)) / 1000);
            displayCountDown = `${day} Ngày ${hour} giờ ${minutes} phút ${second} giây`;
            $('#countDown').html(displayCountDown);
            let objectCountDown = {
                DisplayCountDown: displayCountDown,
                ProductId: $('#ProductId').val()
            }
            connection.invoke("SendCountDown", objectCountDown);
            console.log(displayCountDown);
            if (day == 0 && hour == 0 && minutes == 0 && second == 0) {
                clearInterval(timer);
                // FILL DATA USER WINNER
                // HARD CODE AUCTIONCODE
                // UPDATE STATUS PRODUCT --> COMMING SOON
                // FIND USER BONUS
                let actionCode = $('#AuctionCode').val();
                $.ajax({
                    type: 'POST',
                    url: '/Product/GetMaxAuction',
                    data: {
                        auctionCode: actionCode
                    },
                    success: function (rs) {
                        if (rs.status) {
                            connection.invoke("SetHummanWinner", rs.data);
                            // FIND USER BONUS
                            $.ajax({
                                type: 'POST',
                                url: '/Product/CheckUserGuessWinner',
                                data: {
                                    auctionCode: $('#AuctionCode').val(),
                                    productCode: $('#ProductCode').attr('data-countdown'),
                                    priceStart: $('#initMonney').attr('data-monney'),
                                    priceAuctionSucess: rs.data.bargain
                                },
                                success: function (rss) {
                                    console.log(rss.status)
                                    if (rss.status) {
                                        //
                                        let modelGuessWinner = {
                                            RewardPrediction: rss.rewardPrediction,
                                            Phone: rss.data[0].phone,
                                            ProductCode: rss.data[0].productCode,
                                            AuctionCode: rss.data[0].auctionCode
                                        }
                                        console.log(rss.data);
                                        console.log(modelGuessWinner);
                                        connection.invoke("SetHummanGuessWin", modelGuessWinner);
                                    }
                                }
                            })
                        } else {
                            toastr.error('có lỗi trong hệ thống', 'lỗi rồi!');
                        }
                    }
                });
                                
            }

        }          
    }, 1000);
    // JOIN THE BONUS  
})
var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
function delay(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function numberFormart(n) {
    n = n.replace(/[^0-9]+/g, '');
    return commaSeparateNumber(n);
}
function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}


//Disable send button until connection is established

$('#btnChat').hide();

connection.on("ReceiveMessage", function (objectMess) {
    console.log(objectMess);
    //var msg = objectMess.number.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let encodedMsg = `
         <tr class="itemAuction">
             <td>${objectMess.phone}</td>
             <td>${objectMess.content} VND</td>
             <td>${objectMess.time}</td>
         </tr>
    `;
    let productId = $('#productId').val();
    if (objectMess.productId === productId) {
        $('#tblXepHang').prepend(encodedMsg);
        //$('#tblXepHang').children('tr:first').prepend(encodedMsg);
        let historyAuction = {
            FullNameAuction: objectMess.fullName,
            Bargain: $('#txtMoney').val().replace(/,/g, ''),
            Time: objectMess.time,
            AutionCode: objectMess.auctionCode,
            ProductCode: $('#ProductCode').attr('data-countdown'),
            Phone: objectMess.phone,
            LstPhoneOther: ''
        };
        if (objectMess.fullName == $('#FullNameAuction').val()) {
            $.ajax({
                type: 'POST',
                url: '/Product/SaveHistoryAuction',
                data: {
                    historyAuction: historyAuction
                },
                success: function (rs) {
                    console.log(rs);
                    $('#txtMoney').val('');
                    if (rs.status) {
                        toastr.success('Đã ra giá', 'Thống báo');
                    } else {
                        toastr.error('Lỗi hệ thống', 'Lỗi rồi');
                    }
                }
            });
        }       
    }
});
connection.on("SendGuessWinner", function (object) {
    console.log(object);
    let phone = $('#Phone').val();
    let productCode = $('#ProductCode').attr('data-countdown');
    let auctionCode = $('#AuctionCode').val();
    let bargain = commaSeparateNumber(object.rewardPrediction);
    // SET THÊM AUCTIONCODE
    if (productCode == object.productCode && phone == object.phone && auctionCode == object.auctionCode) {
        console.log(1)
        let notify = `Chúc mừng bạn có SĐT ${object.phone} đã dự đoán đúng!
                Tài khoản của quí khách sẽ công thêm ${bargain} VND
        `;
        $.ajax({
            type: "POST",
            url: '/Product/UpdateMonneyGuessWinner',
            data: {
                monney: object.rewardPrediction,
                phone: object.phone
            },
            success: function (rs) {
                if (rs.status) {
                    toastr.success(`Tài khoản của quí khách đã cộng ${bargain} VND`, 'Thông báo');
                } else {
                    toastr.error('Lỗi hệ thống', 'Lỗi rồi');
                }
            }
        });
        $('#notification').html(notify);
        $('#btnChat').attr('disabled', 'disabled');
        $('#btnSuccess').trigger('click');
        $('#open-modal-aleart-success').show();
        $('#btnSuccessClose').on('click', function () {
            $('#open-modal-aleart-success').hide();
        })
    } 
})
connection.on("SendWinner", function (object) {
    console.log(object);
    let productId = $('#ProductId').val();
    let userName = $('#FullNameAuction').val();
    let productCode = $('#ProductCode').attr('data-countdown');
    let bargain = commaSeparateNumber(object.bargain);
    // SET THÊM AUCTIONCODE
    if (productCode === object.productCode && userName === object.fullNameAuction) {
        console.log(1)
        let notify = `Chúc mừng bạn có SĐT ${object.phone} đã đấu giá thành công!
                Tài khoản của quí khách sẽ bị trừ ${bargain} VND
        `;
        $.ajax({
            type: "POST",
            url: '/Product/UpdateMonneyAcoountWinner',
            data: {
                monney: object.bargain,
                phone: object.phone
            },
            success: function (rs) {
                if (rs.status) {
                    toastr.success(`Tài khoản của quí khách đã trừ ${bargain} VND`, 'Thông báo');
                } else {
                    toastr.error('Lỗi hệ thống', 'Lỗi rồi');
                }
            }
        });
        $('#notification').html(notify);      
        $('#btnChat').attr('disabled', 'disabled');
        $('#btnChat').html('Hết thời gian đấu giá');
        $('#joinBonus').hide();
        $('#btnSuccess').trigger('click');   
        $('#open-modal-aleart-success').show();
        $('#btnSuccessClose').on('click', function () {
            $('#open-modal-aleart-success').hide();
        })
    } else {
        console.log(2)
        let toy = `Chúc bạn may mắn lần sau <3`;
        $('#unnotification').html(toy);
        $('#btnChat').attr('disabled', 'disabled');       
        $('#btnChat').html('Hết thời gian đấu giá');
        $('#joinBonus').hide();
        $('#btnUnSuccess').trigger('click');
        $('#open-modal-aleart-unsuccess').show();
        $('#btnUnSuccessClose').on('click', function () {
            $('#open-modal-aleart-unsuccess').hide();
        })      
    }  
})
//connection.on("GetMonneyMax", function (objectMess) {
//    let productId = $('#productId').val();
//    if (objectMess.productId === productId) {
//        $('#maxMonney').html(`${objectMess.monney}` + ' VND');
//    }
//});
connection.on("MonneyMax", function (objectMess) {
    let productId = $('#productId').val();
    if (objectMess.productId === productId) {
        $('#maxMonney').html(`${objectMess.monney}` + ' VND');
    }
});
connection.on("TimeDown", function (timer) {
    let productId = $('#productId').val();
    if (timer.productId === productId) {
        $('#countDown').html(timer.displayCountDown);
    }
});
connection.start().then(function () {
    $('#btnChat').show();
    //let objectGetMonneyMax = {
    //    Monney: $('#maxMonney').text(),
    //    ProductId: $('#ProductId').val()
    //};
    //connection.invoke("SetMonneyMax", objectGetMonneyMax).catch(function (err) {
    //    return console.error(err.toString());
    //});
}).catch(function (err) {
    return console.error(err.toString());
});
$('#joinBonus').off('click').on('click', function () {
    $.ajax({
        type: 'POST',
        url: '/Product/CheckJoinBonus',
        data: {
            id: $('#ProductId').val()
        },
        success: function (rs) {
            if (rs.status) {
                $('#readJoinB').trigger('click');
            } else {
                toastr.error('Sản phẩm ko cho dự đoán', 'Thông báo');
            }
        }
    })
})
$('#saveBonus').off('click').on('click', function () {
    $.ajax({
        type: 'POST',
        url: '/Product/CreateGuess',
        data: {
            PriceGuess: $('#priceGuess').val().replace(/,/g, ''),
            TotalUserGuess: $('#totalUserGuess').val().replace(/,/g, ''),
            AuctionCode: $('#AuctionCode').val(),
            ProductCode: $('#ProductCode').attr('data-countdown'),
            Phone: '',
            Time: ''
        },
        success: function (rs) {
            if (rs.status) {
                toastr.success('Dự đoán thành công', 'Thông báo');
            } else {
                toastr.error('Không dự đoán được', 'Thông báo');
            }
        }
    });
});
$('#btnChat').off('click').on('click', function () {
    //var phone = $('#txtPhone').val();
    let v = $('#txtMoney').val().replace(/,/g, '');
    let monney = $('#txtMoney').val();
    let productId = $('#productId').val();
    let monneyRequired = $('#minMonney').attr('data-monney').replace(/,/g, '');
    let maxMonneyReq = $('#maxMonney').html().replace(/,/g, '').replace(/VND/g, '');
    if (v < 0 || v > 999999999999 || v == 0) {
        $('#txtMonney').val('');
        toastr.error('Số tiền không hợp lệ', 'Gặp lỗi!')
        return false;
    }
    if (+v < ((+maxMonneyReq) + (+monneyRequired))) {
        toastr.error('Số tiền không hợp lệ', 'Gặp lỗi!')
        return false;
    }
    let objectMess = {
        Content: monney,
        ProductId: productId,
        ProductCode: $('#ProductCode').attr('data-countdown'),
        AuctionCode: $('#AuctionCode').val()
    }
    connection.invoke("SendMessage", objectMess).catch(function (err) {
        return console.error(err.toString());
    });
    let objectMonney = {
        Monney: monney,
        ProductId: productId
    }
    $('#monneyCurrent').html(`${monney}` + " VND");
    connection.invoke("SendMonneyMax", objectMonney);  
})