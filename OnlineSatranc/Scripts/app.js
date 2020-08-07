$(function () {
    $("#macBul").click(function () {
        
        var saniye = 0, dakika = 0, saat = 0, music = true, dongu = setInterval(bak, 1000);

        function bak() {
            $.ajax({
                type: 'post',
                url: "/eslestir/",
                dataType: 'json',
                success: function (data) {
                    if (data.odaID == 0) {
                        function sayac() {
                            if (saniye < 59) saniye = saniye + 1;
                            else {
                                saniye = 0;
                                if (dakika < 59) dakika = dakika + 1;
                                else { dakika = 0; saat = saat + 1; }
                            }
                            $("#eslestirme").html("<span class = 'badge badge-secondary'>MAÇ ARANIYOR<br />" + saat + " : " + dakika + " : " + saniye + "</span>");
                        }
                        $("#eslestirme").html("<span class = ''>0 : 0 : 0</span>");

                        sayac();
                    } else {
                        clearInterval(dongu);
                        var audio = document.getElementById("music");
                        if (music) audio.play();
                        music = false;
                        $("#macBul").fadeOut(2500);
                        setTimeout(
                            function () {
                                $("#eslestirme").html('<button type = "button" class= "btn btn-success btn-lg" id = "macKabul" data-id="' + data.odaID + '" > Kabul Et </button >');  
                            }, 3000);
                    }
                },
                error: function (hata, ajaxOptions, thrownError) {
                    $("#eslestirme").html("<font color = 'red'><b>Eşleştirme Sunucusunda bir sorun meydana geldi! Rahatsızlık için özür dileriz tekrar deneyin :(</b></font>");
                    $("#eslestirme").html("<font color = 'red'><b>" + hata.status + "</b></font>");
                    $("#eslestirme").html("<font color = 'red'><b>" + thrownError + "</b></font>");
                    $("#eslestirme").html("<font color = 'red'><b>" + hata.responseText + "</b></font>");
                }
            });
        }

    });

    $("#macKur").click(function () {

        var bot = false;
        if ($("#macKur").data("bot") == "true") { bot = true;}

        $.ajax({
            type: 'post',
            url: "/kur/",
            data: { 'BOT':  bot},
            dataType: 'json',
            success: function (data) {
                if (data.odaID == 0) {
                    alert("Oda kurulamadı!");
                } else {
                    $("#eslestirme").html("<span class='badge badge-success'>MAÇ KURULUYOR</span>").hide().fadeIn(2500);

                    setTimeout(function () { location.href = "oda/" + data.odaID }, 3000); // 3 saniye sonra yönlendir
                }
            },
            error: function (hata, ajaxOptions, thrownError) {
                $("#eslestirme").html("<font color = 'red'><b>Oda açma sunucusunda bir sorun meydana geldi! Rahatsızlık için özür dileriz tekrar deneyin :(</b></font>");
                $("#eslestirme").html("<font color = 'red'><b>" + hata.status + "</b></font>");
                $("#eslestirme").html("<font color = 'red'><b>" + thrownError + "</b></font>");
                $("#eslestirme").html("<font color = 'red'><b>" + hata.responseText + "</b></font>");
            }
        });

    });

    $("#eslestirme").on("click", "#macKabul", function () {

        var odaNumarasi = $(this).data("id");
        $("#eslestirme").html("<span class='badge badge-success'>MAÇ BAŞLIYOR</span>").hide().fadeIn(2500);

        setTimeout(function () { location.href = "oda/" + odaNumarasi }, 3000); // 3 saniye sonra yönlendir

    });

    $("table tr td").click(function (e) {

        if ($(".satrancTahtasi").find("div").hasClass("seciliKutu1")) { // İKİNCİ TIKLAMA
            
          $(this).find("div").addClass("seciliKutu2");

            if ($(".satrancTahtasi").find(".seciliKutu1").children().data("tasrenk") != $(".satrancTahtasi").find(".seciliKutu2").children().data("tasrenk")) {

                var pozisyon = $(".satrancTahtasi").find(".seciliKutu1").attr("id");
                var tasPrefix = $(".satrancTahtasi").find(".seciliKutu1").children().data("tasprefix");
                var yeniPozisyon = $(".satrancTahtasi").find(".seciliKutu2").attr("id");
                var yenenTas = "-";
                
                if ($(".satrancTahtasi").find(".seciliKutu2").children().is("img")) { // TAŞ YİYORSA

                    if (tasPrefix == "") tasPrefix = pozisyon;
                    var notasyon = tasPrefix + "x" + yeniPozisyon;
                    yenenTas = $(".satrancTahtasi").find(".seciliKutu2").children().data("tasprefix");

                } else { // YER DEĞİŞTİRİYORSA

                    var notasyon = tasPrefix + "" + yeniPozisyon;

                }

                var odaNo = $(".satrancTahtasi").data("odaid");
                var renk = $(".satrancTahtasi").find(".seciliKutu1").children().data("tasrenk");
                var tasRenk;

                if (renk == "beyaz") tasRenk = true;
                else if (renk == "siyah") tasRenk = false;


                var vals = {
                    'notasyon': notasyon,
                    'pozisyon': pozisyon,
                    'odaNo': odaNo,
                    'yenenTas': yenenTas
                }

                $.ajax({

                    type: 'post',
                    url: '/notasyonGonder/',
                    data: vals,
                    dataType: 'json',
                    success: function (data) {
                        if (data.sonuc === true) {
                            if ($(".satrancTahtasi").find(".seciliKutu2").children().is("img")) {
                                $(".satrancTahtasi").find(".seciliKutu1").children().appendTo($(".satrancTahtasi").find(".seciliKutu2")).hide().fadeIn(333);
                                $(".satrancTahtasi").find(".seciliKutu2").children().first().remove();
                            }
                            /*else if ($(".satrancTahtasi").find(".seciliKutu2").children().attr("id").charAt(1) == '8' || $(".satrancTahtasi").find(".seciliKutu2").children().attr("id").charAt(1) == '1') {
                                if (tasRenk === true)
                                    $(".satrancTahtasi").find(".seciliKutu2").children().html("<img src=\"~/Content/img/taslar/beyazVezir.png\" id=\"tasBoyutu\" data-tasprefix=\"\" data-tasrenk=\"beyaz\" />");
                                else $(".satrancTahtasi").find(".seciliKutu2").children().html("<img src=\"~/Content/img/taslar/vezir.png\" id=\"tasBoyutu\" data-tasprefix=\"\" data-tasrenk=\"siyah\" />");
                                $(".satrancTahtasi").find(".seciliKutu1").children().first().remove();
                            }*/
                            else $(".satrancTahtasi").find(".seciliKutu1").children().appendTo($(".satrancTahtasi").find(".seciliKutu2")).hide().fadeIn(333);
                        }
                        else {
                            alert("Geçersiz Hamle! => Notasyon: " + notasyon + " | Pozisyon: " + pozisyon + " | Oda No: " + odaNo);
                        }
                    },
                    error: function (hata, ajaxOptions, thrownError) {
                        alert("Rakip bağlanmadı veya Oyun Kapandı!");
                    }

                });

            } else alert("Kendi taşını yiyemezsin :)");

            setTimeout(function () {
                $(".satrancTahtasi").find(".seciliKutu2").removeClass("seciliKutu2");
                $(".satrancTahtasi").find(".seciliKutu1").removeClass("seciliKutu1");
            }, 500);
        }
        else { // İLK TIKLAMA
            $(this).find("div").addClass("seciliKutu1");
            var vals = {
                'ID': $(".row").data("odasahip"),
                'odaNo': $(".satrancTahtasi").data("odaid")
            }

            $.ajax({

                type: 'post',
                url: '/odaKontrol/',
                data: vals,
                dataType: 'json',
                success: function (data) {
                    if (data.odaSahibi === true && data.odaRakip === false) { // ODA SAHİBİ => BEYAZ TAŞLARLA HAMLE YAPABİLİR

                        if ($(".satrancTahtasi").find(".seciliKutu1").children().data("tasrenk") != "beyaz") {
                            alert("SİYAH TAŞLARLA OYNAMA YETKİNİZ YOK!");
                            $(".satrancTahtasi").find(".seciliKutu1").removeClass("seciliKutu1");
                        }
                        else if (data.hamleSirasi === false) {
                            alert("SIRA SİZDE DEĞİL!");
                            $(".satrancTahtasi").find(".seciliKutu1").removeClass("seciliKutu1");
                        }
                        
                    }
                    else if (data.odaSahibi === false && data.odaRakip === true) { // RAKİP => SİYAH TAŞLARLA HAMLE YAPABİLİR

                        if ($(".satrancTahtasi").find(".seciliKutu1").children().data("tasrenk") != "siyah") {
                            alert("BEYAZ TAŞLARLA OYNAMA YETKİNİZ YOK!");
                            $(".satrancTahtasi").find(".seciliKutu1").removeClass("seciliKutu1");
                        }
                        else if (data.hamleSirasi === false) {
                            alert("SIRA SİZDE DEĞİL!");
                            $(".satrancTahtasi").find(".seciliKutu1").removeClass("seciliKutu1");
                        }
                        
                    }
                    else { // İZLEYİCİ HAMLE YAPAMAZ
                        alert("İZLEYİCİ MODDA HAMLE YAPAMAZSINIZ");
                    }
                },
                error: function (hata, ajaxOptions, thrownError) {
                    alert("Oyun Kapandı!");
                }

            });

        }

    });

    $("#beniHatirla").click(function () {

        var checkBoxes = $("input[name=beniHatirla]");
        checkBoxes.prop("value='true'", checkBoxes.prop("value = 'false'"));
        checkBoxes.prop("value='false'", checkBoxes.prop("value = 'true'"));

    });

    $(document).on('change', '#temaSecim', function () {
        $("#tema").attr('href', "/Content/StyleSheet" + $(this).val() + ".css");
    });

    if (window.location.pathname.substring(1, 4) === "oda") {

        var dongu = setInterval(function () {

            var vals = {

                'odaNo': $(".satrancTahtasi").data("odaid")

            }

            $.ajax({

                type: 'post',
                url: '/rakipGetir/',
                data: vals,
                dataType: 'json',
                success: function (data) {

                    if (data.rakipID != 0) {

                        clearInterval(dongu);

                        var rankPrefix = "";
                        var rankPrefixString = "";

                            if (data.rakipCinsiyet) $("#rakip").find(".card-img-top").attr("src", "/Content/img/avatar/img_avatar1.png");
                            else { $("#rakip").find(".card-img-top").attr("src", "/Content/img/avatar/img_avatar2.png"); rankPrefix = "W"; rankPrefixString = "Woman"; }

                        $("#rakip").find(".card-title").text(data.rakipKadi);

                        if (data.rakipELO > 0 && data.rakipELO < 500) {
                            $("#rakip").find("#rutbeBoyutu").attr("src", "/Content/img/rutbeler/kale.png").removeAttr("hidden");
                            $("#rakip").find("#rutbeText").html("<b>" + rankPrefix + "CM</b> (" + rankPrefixString + " Candidate Master - Usta Adayı)");
                        }
                        else if (data.rakipELO >= 500 && data.rakipELO < 1000) {
                            $("#rakip").find("#rutbeBoyutu").attr("src", "/Content/img/rutbeler/at.png").removeAttr("hidden");
                            $("#rakip").find("#rutbeText").html("<b>" + rankPrefix + "FM</b> (" + rankPrefixString + " FIDE Master - FIDE Ustası)");
                        }
                        else if (data.rakipELO >= 1000 && data.rakipELO < 1500) {
                            $("#rakip").find("#rutbeBoyutu").attr("src", "/Content/img/rutbeler/vezir.png").removeAttr("hidden");
                            $("#rakip").find("#rutbeText").html("<b>" + rankPrefix + "IM</b> (" + rankPrefixString + " International Master - Uluslararası Usta)");
                        }
                        else if (data.rakipELO >= 1500) {
                            $("#rakip").find("#rutbeBoyutu").attr("src", "/Content/img/rutbeler/sah.png").removeAttr("hidden");
                            $("#rakip").find("#rutbeText").html("<b>" + rankPrefix + "GM</b> (" + rankPrefixString + " Grand Master - Büyük Usta)");
                        }

                    }

                },
                error: function (hata, ajaxOptions, thrownError) {
                    alert("Oyun Kapandı!");
                }

            });

        }, 1000);

        setInterval(function () {

                $.ajax({

                    type: 'post',
                    url: '/odaKontrol/',
                    data: {
                        'ID': $(".row").data("odasahip"),
                        'odaNo': $(".satrancTahtasi").data("odaid")
                    },
                    dataType: 'json',
                    success: function (data) {

                        if (data.odaSahibi === true && data.rakipHamle === true || data.odaRakip === true && data.sahipHamle === true) {

                            $.ajax({

                                type: 'post',
                                url: '/notasyonGetir/',
                                data: {
                                    'odaNo': $(".satrancTahtasi").data("odaid")
                                },
                                dataType: 'json',
                                success: function (data) {
                                    var tasCik = data.hamle.tasCik;

                                    if ($(".satrancTahtasi").find("#" + data.hamle.eskiPozisyon).children().is("img")) {
                                        if ($(".satrancTahtasi").find("#" + data.hamle.yeniPozisyon).children().is("img")) {
                                            $(".satrancTahtasi").find("#" + data.hamle.eskiPozisyon).children().appendTo($(".satrancTahtasi").find("#" + data.hamle.yeniPozisyon)).hide().fadeIn(333);
                                            $(".satrancTahtasi").find("#" + data.hamle.yeniPozisyon).children().first().remove();
                                        } else $(".satrancTahtasi").find("#" + data.hamle.eskiPozisyon).children().appendTo($(".satrancTahtasi").find("#" + data.hamle.yeniPozisyon)).hide().fadeIn(333);
                                    }

                                },
                                error: function (hata, ajaxOptions, thrownError) {
                                    alert("Oyun Kapandı!");
                                }

                            });
                        }

                    },
                    error: function (hata, ajaxOptions, thrownError) {
                        alert("Oyun Kapandı!");
                    }

                });

        }, 1000);

    }
    
});