var MultipleVoucher = angular.module("MultipleVoucher", ['ui.bootstrap']);

MultipleVoucher.controller("ApiMultipleVoucherController", function ($scope, $http) {

    var flag = 0;
    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val(); 

        
     
        var transtp = $('#TransType').val();
        var tdate = $('#TransDate').val();
        var monthyear = $('#TransMonthYear').val();
        var tno = $('#Transno').val();

        var inUserID = $('#InUserID').val();
        var insltude = $('#latlonPos').val();



        $http.get('/api/ApiMultipleVoucher/Get', {
            params: {
                Compid: compid,

             
                Transtp: transtp,
                Transdt: tdate,
                Month: monthyear,
                Tno:tno,

                InUserID: inUserID,
                InsLtude: insltude

            }
        }).success(function (data) {
            var TransSerial = data[0].TransSerial;
            var remarks = data[0].REMARKS;
            if(remarks=="holo na")
            {
                alert("Master Data Entry Wrong");
            }
            else if (TransSerial != 0) {
                //alert("Continue");
                $scope.mtransData = data;



            } else {
                //alert("Continue");
                $scope.mtransData = null;

            }

            document.getElementById('TransFor').focus();

            $scope.loading = false;

        });

    };

    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();

        this.newChild.CompanyID = $('#txtcompid').val();


        //this.newChild.BranchID = $("#BranchID").val();
        this.newChild.Transno = $("#Transno").val();
        this.newChild.TransType = $("#TransType").val();

        this.newChild.TransMonthYear = $("#TransMonthYear").val();
        this.newChild.TransDate = $("#TransDate").val();

       
        this.newChild.TransFor = $('#TransFor').val();


        this.newChild.CostPoolID = $("#CostPoolID").val();
        this.newChild.TripNo = $("#txt_tripNo").val();
        this.newChild.TransactionMode = $("#TransactionMode").val();
        this.newChild.DebitCode = $("#DebitCode").val();

        this.newChild.CreditCode = $("#CreditCode").val();
        this.newChild.CHEQUENO = $("#CHEQUENO").val();

        this.newChild.CHEQUEDT = $("#CHEQUEDT").val();

        this.newChild.Amount = $("#Amount").val();
        this.newChild.REMARKS = $("#REMARKS").val();



        this.newChild.INSUSERID = $('#InUserID').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();

        if (this.newChild.TransType != "" && this.newChild.TransDate != "" && this.newChild.Transno !="") {
            $http.post('/api/ApiMultipleVoucher/AddChildData', this.newChild).success(function (data, status, headers, config) {

                var compid = $('#txtcompid').val();


              
                var transtp = $('#TransType').val();
                var tdate = $('#TransDate').val();
                var monthyear = $('#TransMonthYear').val();
                var tno = $('#Transno').val();

                var inUserID = $('#InUserID').val();
                var insltude = $('#latlonPos').val();



                $http.get('/api/ApiMultipleVoucher/Get', {
                    params: {
                        Compid: compid,

                     
                        Transtp: transtp,
                        Transdt: tdate,
                        Month: monthyear,
                        Tno: tno,

                        InUserID: inUserID,
                        InsLtude: insltude

                    }
                }).success(function (data) {
                    var TransSerial = data[0].TransSerial;


                    if (TransSerial != 0) {
                        //alert("Continue");
                        $scope.mtransData = data;



                    } else {
                        //alert("Continue");
                        $scope.mtransData = null;

                    }

                    //document.getElementById('TransactionDate').focus();

                    $scope.loading = false;

                });

                $("#TransFor").val("");

                $("#CostPoolID").val("");
                $("#TransactionMode").val("");

                $('#DebitCode').val("");
                $('#CreditCode').val("");

                $('#CHEQUENO').val(""); 
                $('#CHEQUEDT').val(""); 
                $('#Amount').val("");
                $('#REMARKS').val("");



            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {


            alert("Enter Master Data First");
        }

    };


    $scope.toggleEdit = function () {
        this.item.editMode = !this.item.editMode;

        var changedtxt = $('#TransType').val();
        $.ajax
        ({
            url: '/MultipleVoucher/Debitcdload',
            type: 'post',
            dataType: "json",
            data: { type: changedtxt },
            success: function (data) {
                $("#griddebit").empty();


                $("#griddebit").append('<option value="'
                    + '">'
                    + "--Select--" + '</option>');



                $.each(data, function (i, memo) {

                    $("#griddebit").append('<option value="'
                        + memo.Value + '">'
                        + memo.Text + '</option>');


                });


            }
        });
        $.ajax
               ({
                   url: '/MultipleVoucher/Creditload',
                   type: 'post',
                   dataType: "json",
                   data: { type: changedtxt },
                   success: function (data1) {

                       $("#gridcredit").empty();


                       $("#gridcredit").append('<option value="'
                           + '">'
                           + "--Select--" + '</option>');

                       $.each(data1, function (i, memo1) {

                           $("#gridcredit").append('<option value="'
                               + memo1.Value + '">'
                               + memo1.Text + '</option>');


                       });

                   }
               });


        $('#griddebit').change(function () {

            var debitvalue = $('#griddebit').val();
            $.ajax
            ({
                url: '/MultipleVoucher/CreditloadAfterDebitselect',
                type: 'post',
                dataType: "json",
                data: { type: changedtxt, dvalue: debitvalue },
                success: function (data1) {

                    $("#gridcredit").empty();


                    $("#gridcredit").append('<option value="'
                        + '">'
                        + "--Select--" + '</option>');

                    $.each(data1, function (i, memo1) {

                        $("#gridcredit").append('<option value="'
                            + memo1.Value + '">'
                            + memo1.Text + '</option>');


                    });

                }
            });


        });


    };
    $scope.cancel = function () {
        this.item.editMode = !this.item.editMode;

        var compid = $('#txtcompid').val();


    
        var transtp = $('#TransType').val();
        var tdate = $('#TransDate').val();
        var monthyear = $('#TransMonthYear').val();
        var tno = $('#Transno').val();

        var inUserID = $('#InUserID').val();
        var insltude = $('#latlonPos').val();



        $http.get('/api/ApiMultipleVoucher/Get', {
            params: {
                Compid: compid,

            
                Transtp: transtp,
                Transdt: tdate,
                Month: monthyear,
                Tno: tno,

                InUserID: inUserID,
                InsLtude: insltude

            }
        }).success(function (data) {
            var TransSerial = data[0].TransSerial;
            var remarks = data[0].REMARKS;
            if (remarks == "holo na") {
                alert("Master Data Entry Wrong");
            }
            else if (TransSerial != 0) {
               
                $scope.mtransData = data;



            } else {
               
                $scope.mtransData = null;

            }

            document.getElementById('TransFor').focus();

            $scope.loading = false;

        });


    };

    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.item;

        this.item.CompanyID = $('#txtcompid').val();


     
        this.item.Transno = $("#Transno").val();
        this.item.TransType = $("#TransType").val();

        this.item.TransMonthYear = $("#TransMonthYear").val();
        this.item.TransDate = $("#TransDate").val();
        this.item.UPDLTUDE = $('#latlonPos').val();
        this.item.UPDUSERID = $('#InUserID').val();
      

        $http.post('/api/ApiMultipleVoucher/SaveData', this.item).success(function (data) {
        
                alert("Saved Successfully!!");
            

            frien.editMode = false;
            var compid = $('#txtcompid').val();


      
            var transtp = $('#TransType').val();
            var tdate = $('#TransDate').val();
            var monthyear = $('#TransMonthYear').val();
            var tno = $('#Transno').val();

            var inUserID = $('#InUserID').val();
            var insltude = $('#latlonPos').val();



            $http.get('/api/ApiMultipleVoucher/Get', {
                params: {
                    Compid: compid,

                 
                    Transtp: transtp,
                    Transdt: tdate,
                    Month: monthyear,
                    Tno: tno,

                    InUserID: inUserID,
                    InsLtude: insltude

                }
            }).success(function (data) {
                var TransSerial = data[0].TransSerial;
                var remarks = data[0].REMARKS;
                if (remarks == "holo na") {
                    alert("Master Data Entry Wrong");
                }
                else if (TransSerial != 0) {
                   
                    $scope.mtransData = data;



                } else {
                  
                    $scope.mtransData = null;

                }

                document.getElementById('TransFor').focus();

                //$scope.loading = false;

            });
          

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };


    $scope.deleteitem = function () {
        $scope.loading = true;
        var id = this.item.ID;
        //var branchid = this.item.BRANCHID;
        $http.post('/api/ApiMultipleVoucher/DeleteData', this.item).success(function (data) {

            $.each($scope.mtransData, function (i) {
                if ($scope.mtransData[i].ID === id) {
                    $scope.mtransData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };


    $scope.GetSummOfAmount = function (mtransData) {
        var summ = 0;
        //var scharge = 0;
        //var gross = 0;
        for (var i in mtransData) {
            summ = summ + Number(mtransData[i].Amount);
        }
        //scharge = summ * .2;
        //gross = summ + scharge;
        $('#gridTotalamount').val(summ);
        //$('#SCHARGE').val(scharge);
        //$('#GROSSAMT').val(gross);


        return summ;
    };

});