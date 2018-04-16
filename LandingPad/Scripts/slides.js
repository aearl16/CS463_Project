function loadSlide(current, next) {
    $("div." + current).addClass("collapse");
    $("div." + next).removeClass("collapse");
}

function loadSlideAndConfirm(current, next, pseudonyms, formatTags) {
    $("div." + current).addClass("collapse");
    $("div." + next).removeClass("collapse");

    $("#confirmTitle").empty();
    $("#confirmTitle").append($("#title").val());

    $("#confirmDescription").empty();
    $("#confirmDescription").append($("#description").val());

    $("#confirmProfileID").empty();
    $("#confirmProfileID").append($("#profileID").val());

    $("#confirmPseudonyms").empty();
    for (var i = 0; i < pseudonyms.length; i++) {
        if ($("#pseudonymContainer input." + pseudonyms[i]).is(':checked')) {
            $("#confirmPseudonyms").append($("#pseudonymContainer span span." + pseudonyms[i]).html());
            $("#confirmPseudonyms").append("<br />");
        }
    }

    $("#confirmFormats").empty();
    for (i = 0; i < formatTags.length; i++) {
        if ($("#formatTagContainer input." + formatTags[i]).is(':checked')) {
            $("#confirmFormats").append($("#formatTagContainer span." + formatTags[i] + " span").html());
            $("#confirmFormats").append("<br />");
        }
    }

    $("#confirmLikes").empty();
    if ($("span.likes input").is(':checked')) 
        $("#confirmLikes").append("True");
    else
        $("#confirmLikes").append("False");

    $("#confirmComments").empty();
    if ($("span.comments input").is(':checked'))
        $("#confirmComments").append("True");
    else
        $("#confirmComments").append("False");

    $("#confirmCritique").empty();
    if ($("span.critique input").is(':checked'))
        $("#confirmCritique").append("True");
    else
        $("#confirmCritique").append("False");

    $("#confirmText").empty();
    $("#confirmText").append($(".ql-editor").html());

    $("#editorContent").val() = $(".ql-editor").html();
}