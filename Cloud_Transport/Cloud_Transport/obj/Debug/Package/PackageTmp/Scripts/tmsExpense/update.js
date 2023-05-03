var TmsExpenseApp = angular.module("TmsExpenseApp", ['ui.bootstrap']);

TmsExpenseApp.controller("ApiTmsExpenseController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {

        $scope.loading = true;
        event.preventDefault();

        //Grid view load 
        var compid = $('#txtcompid').val();
        var mYear = $('#txt_transMonthYear').val();
        var trnsNo = $('#txt_transNo').val();

        $http.get('/api/ApiTmsExpense/GetData/', {
            params: {
                companyID: compid,
                transMonthYear: mYear,
                transNo: trnsNo,
            }
        }).success(function (data) {
            if (data[0].count == 1) {
                $scope.expenseData = null;
            } else {
                $scope.expenseData = data;
            }
            $scope.loading = false;
        });

    };



    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };

    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var compid = $('#txtcompid').val();
        var mYear = $('#txt_transMonthYear').val();
        var trnsNo = $('#txt_transNo').val();

        $http.get('/api/ApiTmsExpense/GetData/', {
            params: {
                companyID: compid,
                transMonthYear: mYear,
                transNo: trnsNo,
            }
        }).success(function (data) {
            $scope.expenseData = data;
            $scope.loading = false;
        });
    };


    //Master data
    $scope.submit = function (event) {
        $scope.loading = false;
        event.preventDefault();

        //Grid view load 
        var compid = $('#txtcompid').val();
        var mYear = $('#txt_transMonthYear').val();
        var trnsNo = $('#txt_transNo').val();

        $http.get('/api/ApiTmsExpense/GetData/', {
            params: {
                companyID: compid,
                transMonthYear: mYear,
                transNo: trnsNo,
            }
        }).success(function (data) {
            $scope.expenseData = data;
            $scope.loading = false;
        });
    };



    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#txtDebitCD').val("-SELECT-");
            $('#txtAmount').val("");
            $('#txtRemarks').val("");
            alert("Permission Denied!!");
        }
        else {

            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.TRANSDT = $('#txt_transDt').val();
            this.newChild.TRANSMY = $('#txt_transMonthYear').val();
            this.newChild.TRANSNO = $('#txt_transNo').val();
            this.newChild.TRIPNO = $('#txt_tripNo').val();
            this.newChild.COSTPID = $('#txt_Costpid').val();

            if (this.newChild.TRANSDT != "" && this.newChild.TRANSNO != "" && this.newChild.TRANSNO != 0) {
                $http.post('/api/ApiTmsExpense/AddData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid = $('#txtcompid').val();
                    var mYear = $('#txt_transMonthYear').val();
                    var trnsNo = $('#txt_transNo').val();

                    $http.get('/api/ApiTmsExpense/GetData/', {
                        params: {
                            companyID: compid,
                            transMonthYear: mYear,
                            transNo: trnsNo,
                        }
                    }).success(function (data) {
                        $scope.expenseData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL != 0) {
                        $('#txtDebitCD').val("-SELECT-");
                        $('#txtAmount').val("");
                        $('#txtRemarks').val("");
                        alert("Create Successfully !!");
                        //$scope.expenseData.push({ ID: data.ID, MCATID: data.MCATID, MEDIID: data.MEDIID, MEDINM: data.MEDINM, PHARMAID: data.PHARMAID, GHEADID: data.GHEADID });
                    } else {
                        $('#txtDebitCD').val("-SELECT-");
                        $('#txtAmount').val("");
                        $('#txtRemarks').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else {
                alert("Please Select grid level data !!");
            }

        }
    };







    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();

        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var compid = $('#txtcompid').val();
            var mYear = $('#txt_transMonthYear').val();
            var trnsNo = $('#txt_transNo').val();

            $http.get('/api/ApiTmsExpense/GetData/', {
                params: {
                    companyID: compid,
                    transMonthYear: mYear,
                    transNo: trnsNo,
                }
            }).success(function (data) {
                $scope.expenseData = data;
            });

            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiTmsExpense/UpdateData', this.testitem).success(function (data) {
                alert("Saved Successfully!!");
                frien.editMode = false;

                //Grid view load 
                var compid = $('#txtcompid').val();
                var mYear = $('#txt_transMonthYear').val();
                var trnsNo = $('#txt_transNo').val();

                $http.get('/api/ApiTmsExpense/GetData/', {
                    params: {
                        companyID: compid,
                        transMonthYear: mYear,
                        transNo: trnsNo,
                    }
                }).success(function (data) {
                    $scope.expenseData = data;
                });

                $scope.loading = false;

            }).error(function (data) {
                $scope.error = "An Error has occured while Saving Friend! " + data;
                $scope.loading = false;

            });
        }
    };





    //Delete grid level data.
    //$scope.deleteItem = function () {
    //    $scope.loading = true;
    //    var id = this.testitem.ID;

    //    var Delete = $('#txt_Deleteid').val();
    //    if (Delete == "I") {
    //        alert("Permission Denied!!");
    //    }
    //    else {
    //        this.testitem.COMPID = $('#txtcompid').val();
    //        this.testitem.INSUSERID = $('#txtInsertUserid').val();
    //        this.testitem.INSLTUDE = $('#latlon').val();

    //        $http.post('/api/ApiTmsExpense/DeleteData/', this.testitem).success(function (data) {

    //            var changedText = $("#txt_REGNID").val();
    //            var registrationDate = $("#txt_REGNDT").val();
    //            var compid = $('#txtcompid').val();
    //            $("#CItemNameid").val(" ");
    //            $.ajax
    //            ({
    //                url: '/RegistrationComplementaryItem/ComplementaryItemLoad',
    //                type: 'post',
    //                dataType: "json",
    //                data: { regID: changedText, theDate: registrationDate, companyid: compid },
    //                success: function (data) {
    //                    $("#CItemNameid").empty();
    //                    $("#CItemNameid").append('<option value="'
    //                        + "Select" + '">'
    //                        + "Select" + '</option>');


    //                    $.each(data, function (i, memo) {

    //                        $("#CItemNameid").append('<option value="'
    //                            + memo.Value + '">'
    //                            + memo.Text + '</option>');

    //                    });
    //                }
    //            });



    //            $.each($scope.expenseData, function (i) {
    //                if ($scope.expenseData[i].ID === id) {
    //                    $scope.expenseData.splice(i, 1);
    //                    return false;
    //                }
    //            });
    //            $scope.loading = false;
    //            alert("Delete Successfully!!");

    //        }).error(function (data) {
    //            $scope.error = "An Error has occured while delete posts! " + data;
    //            $scope.loading = false;
    //        });
    //    }
    //};



    //Delete grid level data.
    $scope.deleteItem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiTmsExpense/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.expenseData, function (i) {
                    if ($scope.expenseData[i].ID === id) {
                        $scope.expenseData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };



});