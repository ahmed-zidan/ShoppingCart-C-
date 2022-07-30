$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("confirm deletion")) {
                return false;
            }
        })
    }


    if ($("div.alert.notification").length) {
        setTimeout(() => {

            $("div.alert.notification").fadeOut();

        }, 2000);
    }



})


function readUrl(input) {

    let reader = new FileReader();


    reader.onload = function (e) {
        $("img#imgUpload").attr("src", e.target.result).width(200).height(200);
    }

    reader.readAsDataURL(input.files[0]);

}
