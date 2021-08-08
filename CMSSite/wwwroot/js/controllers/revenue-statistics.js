let selectionUrl = "/SelectionData/";
var mixedChart = null;
var horizontalBarChart = null;
var donutChart = null;
const unitValue = 1000000000;
var model = {};

$(function () {
    $('.section_dashboard .search .form_datetime#date-picker').datetimepicker('setDate', new Date());

    // DASHBOARD - toggle handle
    $(".section_dashboard .toggle-handle").on("click", function () {
        const handle = $(".section_dashboard .handle-container");
        $(handle).toggleClass("hide");
        $(this).toggleClass("open");
        if ($(".section_dashboard .handle-container").hasClass('hide')) {
            $('#statis-revenue').trigger('click');
        }
    });

    //load data in levels selections
    $('#branch-select').on('change', function () {
        var branch = $('#branch-select').val();
        if (branch) {
            LoadOfficesToForm('#office-select', branch);
        }
        else {
            $('#office-select').html('');
        }

        $('#office-select').html('<option value="">--</option>');
        $('#depart-select').html('<option value="">--</option>');
        $('#team-select').html('<option value="">--</option>');
        $('#staff-select').html('<option value="">--</option>');
    });

    $('#office-select').on('change', function () {
        var office = $('#office-select').val();
        if (office) {
            LoadDeptsToForm('#depart-select', office);
        }
        else {
            $('#depart-select').html('');
        }

        $('#depart-select').html('<option value="">--</option>');
        $('#team-select').html('<option value="">--</option>');
        $('#staff-select').html('<option value="">--</option>');
    });

    $('#depart-select').on('change', function () {
        var dept = $('#depart-select').val();
        if (dept) {
            LoadTeamsToForm('#team-select', dept);
        }
        else {
            $('#team-select').html('');
        }

        $('#team-select').html('<option value="">--</option>');
        $('#staff-select').html('<option value="">--</option>');
    });

    $('#team-select').on('change', function () {
        var team = $('#team-select').val();
        if (team) {
            LoadStaffsToForm('#staff-select', team);
        }
        else {
            $('#staff-select').html('');
        }
        $('#staff-select').html('<option value="">--</option>');
    });

    //setup selection with account role
    let branch = $('#branch-select').data('branch');
    let office = $('#office-select').data('office');
    let dept = $('#depart-select').data('department');
    let team = $('#team-select').data('team');
    let staff = $('#staff-select').data('staff');
    let role = $('#statis-revenue').data('role');
    if (branch) {
        $.get(selectionUrl + "GetAllBranches?", function (res) {
            if (res.result == 400) {
            }
            else {
                var el = $('#branch-select');
                el.html('');
                $.each(res.data, function (index, item) {
                    if (item.branchCode == branch) {
                        el.append(`<option value="${item.branchCode}">${item.branchName}</option>`);
                    }
                });

                if (office) {
                    $.get(selectionUrl + "GetAllOffice?branch=" + branch, function (res) {
                        var el = $('#office-select');
                        el.html('');
                        if (res.result == 400) {
                        }
                        else {
                            $.each(res.data, function (index, item) {
                                if (item.officeCode == office) {
                                    el.append(`<option value="${item.officeCode}">${item.officeName}</option>`);
                                }
                            });

                            if (dept) {
                                $.get(selectionUrl + "GetDepartments?office=" + office, function (res) {
                                    var el = $('#depart-select');
                                    el.html('');
                                    if (res.result == 400) {
                                    }
                                    else {
                                        $.each(res.data, function (index, item) {
                                            if (item.departmentCode == dept) {
                                                el.append(`<option value="${item.departmentCode}">${item.departmentName}</option>`);
                                            }
                                        });

                                        if (team) {
                                            $.get(selectionUrl + "GetTeams?department=" + dept, function (res) {
                                                var el = $('#team-select');
                                                el.html('');
                                                if (res.result == 400) {
                                                }
                                                else {
                                                    $.each(res.data, function (index, item) {
                                                        if (item.teamCode == team) {
                                                            el.append(`<option value="${item.teamCode}">${item.name}</option>`);
                                                        }
                                                    });

                                                    if (staff) {
                                                        $.get(selectionUrl + "GetStaffSale?teamCode=" + team, function (res) {
                                                            var el = $('#staff-select');
                                                            el.html('');
                                                            if (res.result == 400) {
                                                            }
                                                            else {
                                                                $.each(res.data, function (index, item) {
                                                                    if (item.codeStaff == staff) {
                                                                        el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`);
                                                                    }
                                                                });
                                                                $('#staff-select').val(staff).trigger('change');
                                                                $('#staff-select').attr('disabled', true);
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        $('#team-select').val(team).trigger('change');
                                                    }
                                                    $('#team-select').attr('disabled', true);
                                                }
                                            });
                                        }
                                        else {
                                            $('#depart-select').val(dept).trigger('change');
                                        }
                                        $('#depart-select').attr('disabled', true);
                                    }
                                });
                            }
                            else {
                                $('#office-select').val(office).trigger('change');
                            }
                            $('#office-select').attr('disabled', true);
                        }
                    });
                }
                else {
                    $('#branch-select').val(branch).trigger('change');
                }
                $('#branch-select').attr('disabled', true);
            }
        });
        //model for first time
        model.time = $('#date-picker').val().replaceAll('/', '-');
        model.timeOption = $('#time-option').val();
        model.branch = branch;
        if (office) {
            model.office = office;
            if (dept) {
                model.department = dept;
                if (team) {
                    model.team = team;
                    if (staff) {
                        model.staff = staff;
                    }
                }
            }
        }
    }
    else {
        //model for first time
        LoadBranchesToForm('#branch-select');
        model.time = $('#date-picker').val().replaceAll('/', '-');
        model.timeOption = $('#time-option').val();
    }

    // statis by many coniditions
    $('#statis-revenue').click(function () {
        $('.loading-wrapper').show();
        model.time = $('#date-picker').val().replaceAll('/', '-');
        model.timeOption = $('#time-option').val();

        if (!$('.handle-container').hasClass('hide')) {
            var branch = $('#branch-select').val();
            if (branch) {
                model.branch = branch;

                var office = $('#office-select').val();
                if (office) {
                    model.office = office;

                    var dept = $('#depart-select').val();
                    if (dept) {
                        model.department = dept;

                        var team = $('#team-select').val();
                        if (team) {
                            model.team = team;

                            var staff = $('#staff-select').val();
                            if (staff) {
                                model.staff = staff;
                            }
                            else {
                                model.staff = null;
                            }
                        }
                        else {
                            model.team = null;
                            model.staff = null;
                        }
                    }
                    else {
                        model.department = null;
                        model.team = null;
                        model.staff = null;
                    }
                }
                else {
                    model.office = null;
                    model.department = null;
                    model.team = null;
                    model.staff = null;
                }
            }
            else {
                model.branch = null;
                model.office = null;
                model.department = null;
                model.team = null;
                model.staff = null;
            }
        }
        else {
            let branch = $('#branch-select').data('branch');
            let office = $('#office-select').data('office');
            let dept = $('#depart-select').data('department');
            let team = $('#team-select').data('team');
            let staff = $('#staff-select').data('staff');
            let role = $('#statis-revenue').data('role');

            switch (role) {
                case 1:
                    model.branch = null;
                    model.office = null;
                    model.department = null;
                    model.team = null;
                    model.staff = null;
                    break;

                case 7:
                    model.branch = branch;
                    model.office = null;
                    model.department = null;
                    model.team = null;
                    model.staff = null;
                    break;

                case 10:
                    model.branch = branch;
                    model.office = office;
                    model.department = null;
                    model.team = null;
                    model.staff = null;
                    break;

                case 6:
                    model.branch = branch;
                    model.office = office;
                    model.department = dept;
                    model.team = null;
                    model.staff = null;
                    break;

                case 5:
                    model.branch = branch;
                    model.office = office;
                    model.department = dept;
                    model.team = team;
                    model.staff = null;
                    break;

                case 4:
                case 11:
                    model.branch = branch;
                    model.office = office;
                    model.department = dept;
                    model.team = team;
                    model.staff = staff;
                    break;
            }
        }

        LoadRevenueStatisticalData(model);
    });
    //initial
    LoadRevenueStatisticalData(model);

    $(document).on('click', '.choose-branch', function () {
        let isCheck = $(this).prop("checked");
        if (isCheck) {
        }
        else {
            $(this).closest('div.input-group').find('input.branch-revenue').val('');
        }

        var totalRev = $('#total-revenue').val();
        if (totalRev) {
            //var checkedBranch = $('.choose-branch:checked');
            var checkedBranch = $('.choose-branch');
            if (checkedBranch) {
                let rev = Math.floor(totalRev / checkedBranch.length * 1000) / 1000;
                $(this).closest('div.input-group').find('input.branch-revenue').val(rev).trigger('change');
            }

            //$('#remain-target').val(total);
        }
    });

    $(document).on('change', '.branch-revenue', function () {
        //valid input revenue
        let rev = $(this).val();
        if (rev >= 10000) {
            toastr.error('Doanh thu không thể hớn hơn 10,000 tỷ');
            $(this).val('');
            return;
        }

        //update remain revenue
        var totalRev = $('#total-revenue').val();
        if (totalRev) {
            var checkedBranch = $('.choose-branch');
            if (checkedBranch) {
                var currentTotal = 0;
                $.each(checkedBranch, function (index, item) {
                    let rev = $(item).closest('div.input-group').find('input.branch-revenue').val();
                    if (rev) {
                        currentTotal += parseFloat(rev);
                    }
                });
                if (totalRev >= currentTotal) {
                    $('#remain-target').val(totalRev - currentTotal);
                }
                else {
                    $('#remain-target').val(0);
                }
            }
        }
    });

    $(document).on('change', '#total-revenue', function () {
        let rev = $('#total-revenue').val();
        if (rev >= 10000) {
            toastr.error('Tổng doanh thu không thể hớn hơn 10,000 tỷ');
            $('#total-revenue').val('');
        }
    });

    $('#setup-revenue').click(function () {
        $('.loading-wrapper').show();
        var model = {};
        model.Targets = [];
        var totalRev = $('#total-revenue').val();
        if (totalRev) {
            var inputTargets = $('#target_modal .branch-revenue');
            var currentTotal = 0;
            $.each(inputTargets, function (index, item) {
                let rev = $(item).val();
                if (rev && rev != 0) {
                    currentTotal += parseFloat(rev);
                }
                else {
                    return;
                }

                var target = {};
                target.SetTargetFor = $(item).data('branch');
                target.Revenue = rev;
                model.Targets.push(target);
            });

            if (model.Targets.length == 0) {
                toastr.error('Nhập ít nhất 1 chỉ tiêu cho cấp dưới để thiết lập');
                $('.loading-wrapper').hide();
                return;
            }

            if (totalRev < currentTotal) {
                toastr.error('Tổng chỉ tiêu của các cấp dưới phải nhỏ hơn hoặc bằng tổng chỉ tiêu đề ra');
                $('.loading-wrapper').hide();
                return;
            }

            if (totalRev > currentTotal) {
                toastr.warning('Tổng chỉ tiêu của các cấp dưới chưa đủ tổng chỉ tiêu đề ra');
            }
        }
        else {
            toastr.error('Chưa nhập chỉ tiêu tổng doanh thu');
            $('.loading-wrapper').hide();
            return;
        }

        $.post(baseUrl + 'SetRevenueTarget', model, function (res) {
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
                CloseModal('#target_modal');
                $('#statis-revenue').trigger('click');
                //ClearTarget();
            }
            $('.loading-wrapper').hide();
        });
    });
});

var ClearTarget = function () {
    $('#target_modal input').val('');
    $('#target_modal input.branch-revenue').attr('disabled', true);
    $('#target_modal input.choose-branch').prop('checked', false);
}

var LoadRevenueStatisticalData = function (model) {
    $('.loading-wrapper').show();
    var url = baseUrl + 'GetStatisticsData';
    $.each(model, function (index, item) {
        if (item) {
            url += '/' + item;
        }
    });
    $.get(url, function (res) {
        if (res.result == 200) {
            console.log(res.data);

            //update current revenue, target, grow
            $('#revenue-counting').text((Math.round((res.data.currentRevenue / unitValue) * 100) / 100).toString());
            $('#revenue-counting').attr('data-number', (Math.round((res.data.currentRevenue / unitValue) * 100) / 100).toString());
            $('#grow-counting').text(res.data.proportionPercent.toString());
            $('#grow-counting').attr('data-number', res.data.proportionPercent.toString());
            $('#target-counting').text(res.data.finishedLevel.toString());
            //$('#target-counting').attr('data-number', res.data.finishedLevel.toString());
            //$(".statistic .target").attr('data-number', res.data.finishedLevel.toString());

            // DASHBOARD - warning target
            const statisticTarget = $(".statistic .target");
            //const targetData = $(statisticTarget).attr("data-number");
            if (res.data.finishedLevel <= 20) {
                $(statisticTarget).addClass("warning");
            } else $(statisticTarget).removeClass("warning");

            //show target detail
            $('.level-children').html('');
            var totalRev = 0
            $.each(res.data.targets, function (index, item) {
                let rev = 0;
                if (item.revenue) {
                    rev = item.revenue / unitValue;
                    totalRev += rev;
                }
                $('.level-children').append(
                    `
                        <div class="modal-item input-group">
                            <label class="checkbox-label">
                                <div class="custom-checkbox">
                                    <input type="checkbox" class="choose-branch" />
                                    <span class="checkmark"></span>
                                </div>
                                <span>${item.name}</span>
                            </label>
                            <input data-branch="${item.setTargetFor}" type="number" value="${rev ?? ''}" class="branch-revenue" />
                        </div>
`
                );
            });
            $('#total-revenue').val(totalRev);
            $('#remain-target').val(0);

            $('#cancel-set-target, #target_modal .close__modal').off('click').on('click', function () {
                $('.level-children').html('');
                var totalRev = 0
                $.each(res.data.targets, function (index, item) {
                    let rev = 0;
                    if (item.revenue) {
                        rev = item.revenue / unitValue;
                        totalRev += rev
                    }
                    $('.level-children').append(
                        `
                        <div class="modal-item input-group">
                            <label class="checkbox-label">
                                <div class="custom-checkbox">
                                    <input type="checkbox" class="choose-branch" />
                                    <span class="checkmark"></span>
                                </div>
                                <span>${item.name}</span>
                            </label>
                            <input data-branch="${item.setTargetFor}" type="number" value="${rev ?? ''}" class="branch-revenue" />
                        </div>
`
                    );
                });
                $('#total-revenue').val(totalRev);
                $('#remain-target').val(0);
            });

            // setup data
            // ============mixed==============
            const mixedLabels = [
            ];
            const mixedDataArray = {
                bar: [],
                line: [],
            };
            //add data
            $.each(res.data.allLevelsRevenueInDuration, function (index, item) {
                mixedLabels.push(item.startTime);
                mixedDataArray.bar.push(item.revenue / unitValue);
                mixedDataArray.line.push(item.revenue / unitValue);
            });
            //reset chart
            if (mixedChart != null) {
                mixedChart.destroy();
                $('#mixed-legend').html('');
                $('#mixed').remove();
                $('.mixed-container').append('<canvas id="mixed"></canvas>');
            }

            //=========horizontal chart===============
            const horizontalBarLabels = [];
            const hotizontalBarDataArray = [];
            //add data
            $.each(res.data.currentALevelRevenue, function (index, item) {
                horizontalBarLabels.push(item.name);
                hotizontalBarDataArray.push(item.revenue / unitValue);
            });
            //reset chart
            if (horizontalBarChart != null) {
                horizontalBarChart.destroy();
                $('#horizontal_bar').remove();
                $('.horizontal_bar-container .horizontal_bar').append('<canvas id="horizontal_bar"></canvas>');
            }

            // === donut ===
            const donutLabels = [];
            const donutDataArray = [];

            //add data
            $.each(res.data.productComponents, function (index, item) {
                donutLabels.push(item.productName);
                donutDataArray.push(item.percent);
            });

            //reset chart
            if (donutChart != null) {
                donutChart.destroy();
                $('#donut').remove();
                $('.donut-chart').append('<canvas id="donut" width="160" height="160"></canvas>');
            }

            const donutCtx = $("#donut");
            if (donutCtx) {
                const donutData = {
                    labels: donutLabels,
                    datasets: [
                        {
                            data: donutDataArray,
                            backgroundColor: donutColors,
                            hoverOffset: 3,
                        },
                    ],
                };

                const donutConfig = {
                    type: "doughnut",
                    data: donutData,
                    options: donutOptions(donutTooltipHandler),
                    plugins: [donutLegendPlugin],
                };

                donutChart = new Chart(donutCtx, donutConfig);
            }

            // === mixed ===
            const mixedCtx = $("#mixed");
            if (mixedCtx) {
                const mixedData = {
                    labels: mixedLabels,
                    datasets: [
                        {
                            type: "bar",
                            label: "Doanh so",
                            data: mixedDataArray.bar,
                            backgroundColor: mixedColors.bar,
                            order: 2,
                            barThickness: 20,
                        },
                        {
                            type: "line",
                            label: "Tang truong",
                            data: mixedDataArray.line,
                            borderColor: mixedColors.line,
                            backgroundColor: mixedColors.line,
                            fill: false,
                            order: 1,
                            pointBorderWidth: 4,
                            borderWidth: 2,
                        },
                    ],
                };

                const mixedConfig = {
                    data: mixedData,
                    options: mixedOptions,
                    plugins: [mixedLegendPlugin],
                };
                mixedChart = new Chart(mixedCtx, mixedConfig);
            }

            // === horizontal ===

            const horizontalBarCtx = $("#horizontal_bar");
            if (horizontalBarCtx) {
                // update horizontal
                horizontalBarCtx.css("height", horizontalBarLabels.length * 20);
                // end update horizontal
                const horizontalBarData = {
                    labels: horizontalBarLabels,
                    datasets: [
                        {
                            data: hotizontalBarDataArray,
                            backgroundColor: generateArrayOfColor(horizontalBarLabels.length),
                        },
                    ],
                };

                const horizontalBarConfig = {
                    type: "bar",
                    data: horizontalBarData,
                    options: horizontalBarOptions,
                    // update
                    plugins: [ChartDataLabels]
                    // end update
                };

                horizontalBarChart = new Chart(
                    horizontalBarCtx,
                    horizontalBarConfig
                );
            }

            //update lable chart
            $('#target_modal input').attr('disabled', false);
            $('#target_modal input[type="checkbox"]').prop('checked', false);
            $('#target_modal input#remain-target').attr('disabled', true);
            $('#target_modal #setup-revenue').hide();
            
            let role = $('#statis-revenue').data('role');
            if (!$('.handle-container').hasClass('hide')) {
                var staff = $('#staff-select').val();
                var team = $('#team-select').val();
                if (staff || team) {
                    $('.scale.x').text('Sale');
                    $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng sale');
                    if (role == 5) { $('#target_modal #setup-revenue').show(); }
                    if (staff) {
                        $('#target_modal input').attr('disabled', true);
                        $('#target_modal #setup-revenue').hide();
                    }
                }
                else {
                    var dept = $('#depart-select').val();
                    if (dept) {
                        $('.scale.x').text('Nhóm');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng nhóm');
                        if (role == 6) { $('#target_modal #setup-revenue').show(); }
                    }
                    else {
                        var office = $('#office-select').val();
                        if (office) {
                            $('.scale.x').text('Phòng');
                            $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng phòng ban');
                            if (role == 10) { $('#target_modal #setup-revenue').show(); }
                        }
                        else {
                            var branch = $('#branch-select').val();
                            if (branch) {
                                $('.scale.x').text('Khối');
                                $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng khối');
                                if (role == 7) { $('#target_modal #setup-revenue').show(); }
                            }
                            else {
                                $('.scale.x').text('Chi nhánh');
                                $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng chi nhánh');
                                if (role == 1) { $('#target_modal #setup-revenue').show(); }
                            }
                        }
                    }
                }
            }
            else {
                $('#target_modal #setup-revenue').show();
                switch (role) {
                    case 1:
                        $('.scale.x').text('Chi nhánh');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng chi nhánh');
                        break;

                    case 7:
                        $('.scale.x').text('Khối');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng khối');
                        break;

                    case 10:
                        $('.scale.x').text('Phòng');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng phòng ban');
                        break;

                    case 6:
                        $('.scale.x').text('Nhóm');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng nhóm');
                        break;

                    case 5:
                        $('.scale.x').text('Sale');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng sale');
                        break;

                    case 4:
                    case 11:
                        $('.scale.x').text('Sale');
                        $('.horizontal_bar-container .chart-title .title').text('Doanh thu của từng sale');
                        $('#target_modal input').attr('disabled', true);
                        break;
                }
            }

            $('.loading-wrapper').hide();
        }
        else {
            toastr.error(res.errors.join('<br />'));
            $('.loading-wrapper').hide();
        }
    });
}

var LoadBranchesToForm = function (target, selected = null) {
    $.get(selectionUrl + "GetAllBranches?", function (res) {
        if (res.result == 400) {
        }
        else {
            var el = $(target);
            el.html('');
            el.append(`<option value="">--</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.branchCode}">${item.branchName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}

var LoadOfficesToForm = function (target, branch, selected = null) {
    $.get(selectionUrl + "GetAllOffice?branch=" + branch, function (res) {
        var el = $(target);
        el.html('');
        el.append(`<option value="">--</option>`);
        if (res.result == 400) {
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.officeCode}">${item.officeName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}

var LoadDeptsToForm = function (target, office, selected = null) {
    $.get(selectionUrl + "GetDepartments?office=" + office, function (res) {
        var el = $(target);
        el.html('');
        el.append(`<option value="">--</option>`);
        if (res.result == 400) {
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.departmentCode}">${item.departmentName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}

var LoadTeamsToForm = function (target, dept, selected = null) {
    $.get(selectionUrl + "GetTeams?department=" + dept, function (res) {
        var el = $(target);
        el.html('');
        el.append(`<option value="">--</option>`);
        if (res.result == 400) {
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.teamCode}">${item.name}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}

var LoadStaffsToForm = function (target, team, selected = null) {
    $.get(selectionUrl + "GetStaffSale?teamCode=" + team, function (res) {
        var el = $(target);
        el.html('');
        el.append(`<option value="">--</option>`);
        if (res.result == 400) {
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}