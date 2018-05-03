﻿/**
 * Scripts for dealing with the "slides" on the _Menu partial view
 * and the partial views that are derived from it
 * Also populates the confirmation fields and some of the hidden
 * form fields that allow the controller to access the information
 * from the submitted form
 *
 * Finally, features a small function to check if an element is
 * empty using jQuery
 */

/**
 * Interacts with the _Menu partial view to switch
 * between the different "pages" that are used to enter
 * information when creating or editing a writing.
 * Works by passing the class of the current (noncollapsed)
 * slide and the class of the next slide (the collapsed slide
 * we want to go to) and adding the collapse class to the current
 * slide and removing the collapse class from the next slide.
 * Can be used to go "back" by inserting the class name of the
 * previous slide into the parameter next.
 * @param {string} current (class name)
 * @param {string} next (class name)
 */
function loadSlide(current, next) {
    //collapse the current slide and uncollapse the next slide
    $("div." + current).addClass("collapse");
    $("div." + next).removeClass("collapse");
}

/**
 * Works the same way as the previous function to switch between slides,
 * but also populates the various spans and divs on the confirmation "page"
 * with the current values so that the user can decide whether the values
 * they are using are correct before saving. Uses a different function name
 * rather than a function overload because apparently trying to overload a
 * function in JavaScript makes your code explode.
 * @param {string} current (class name)
 * @param {string} next (class name)
 * @param {int[]} pseudonyms (an array of all the PseudonymIDs; from the view's Controller)
 * @param {int[]} formatTags (an array of all the FormatTagIDs; from the view's Controller)
 */
function loadSlideAndConfirm(current, next, pseudonyms, formatTags) {
    //collapse the current slide and uncollapse the next slide as usual
    $("div." + current).addClass("collapse");
    $("div." + next).removeClass("collapse");

    //empty the field for title confirmation, then append the value of the title input
    $("#confirmTitle").empty();
    $("#confirmTitle").append($("#title").val());

    //empty the field for the description confirmation, then append the value of the description input
    $("#confirmDescription").empty();
    $("#confirmDescription").append($("#description").val());

    //empty the field for the ProfileID confirmation, then append the value of the ProfileID select
    $("#confirmProfileID").empty();
    $("#confirmProfileID").append($("#profileID").val());

    //empty the field for the pseudonym confirmation
    $("#confirmPseudonyms").empty();
    //for each pseudonym in the database
    for (var i = 0; i < pseudonyms.length; i++) {
        //if the checkbox for the current pseudonym is checked
        if ($("#pseudonymContainer input." + pseudonyms[i]).is(':checked')) {
            //append the value of pseudonym name that matches this checkbox to the confirmation field
            $("#confirmPseudonyms").append($("#pseudonymContainer span span." + pseudonyms[i]).html());
            //append a break so everything is still legible 
            $("#confirmPseudonyms").append("<br />");
        }
    }

    //empty the field for confirming whether or not pseudonyms are used in addition to the username
    $("#confirmUseInAdditionToUsername").empty();
    //this checkbox only shows up if at least one pseudonym has been checked
    //this is because the username is automatically used if there are no pseudonyms selected
    //if the checkbox for using pseudonyms in addition to the username is checked
    if ($("#usePseudonymsInAdditionToUsername").is(':checked')) //put the word "Yes" in the field
        $("#confirmUseInAdditionToUsername").append("Yes");
    else //if the checkbox is not checked, put the word "No" in the field
        $("#confirmUseInAdditionToUsername").append("No");

    //empty the field for format tag confirmation
    $("#confirmFormats").empty();
    //for each format tag in the database
    for (i = 0; i < formatTags.length; i++) {
        //if the current format tag has been checked
        if ($("#formatTagContainer input." + formatTags[i]).is(':checked')) {
            //append the value of the format tag name that matches this checkbox to the confirmation field
            $("#confirmFormats").append($("#formatTagContainer span." + formatTags[i] + " span").html());
            //append a break so everything is still legible 
            $("#confirmFormats").append("<br />");
        }
    }

    //empty the field for public access confirmation
    $("#confirmPublicAccess").empty();
    //if the checkbox for allowing public access is checked, append true
    if ($("span.public input").is(':checked'))
        $("#confirmPublicAccess").append("True");
    else //otherwise, append false
        $("#confirmPublicAccess").append("False");

    //empty the field for friend access confirmation
    $("#confirmFriendAccess").empty();
    //if the checkbox for allowing friend access is checked, append true
    if ($("span.friend input").is(':checked'))
        $("#confirmFriendAccess").append("True");
    else //otherwise, append false
        $("#confirmFriendAccess").append("False");

    //empty the field for publisher access confirmation
    $("#confirmPublisherAccess").empty();
    //if the checkbox for allowing publisher access is checked, append true
    if ($("span.publisher input").is(':checked'))
        $("#confirmPublisherAccess").append("True");
    else //otherwise, append false
        $("#confirmPublisherAccess").append("False");

    //empty the field for minor access confirmation
    $("#confirmMinorAccess").empty();
    //if the checkbox for allowing minor access is checked, append true
    if ($("span.minor input").is(':checked'))
        $("#confirmMinorAccess").append("True");
    else //otherwise, append false
        $("#confirmMinorAccess").append("False");

    //empty the field for like confirmation
    $("#confirmLikes").empty();
    //if the checkbox for allowing likes is checked, append true
    if ($("span.likes input").is(':checked')) 
        $("#confirmLikes").append("True");
    else //otherwise, append false
        $("#confirmLikes").append("False");

    //empty the field for comment confirmation
    $("#confirmComments").empty();
    //if the checkbox for allowing comments is checked, append true
    if ($("span.comments input").is(':checked'))
        $("#confirmComments").append("True");
    else //otherwise, append false
        $("#confirmComments").append("False");

    //empty the field for critique confirmation
    $("#confirmCritique").empty();
    //if the checkbox for allowing critique is checked, append true
    if ($("span.critique input").is(':checked'))
        $("#confirmCritique").append("True");
    else //otherwise, append false
        $("#confirmCritique").append("False");

    //this field is for use on the normal create and edit pages where you are working with the editor
    //empty the field for text content confirmation
    $("#confirmText").empty();
    //take the formatted text from the editor and put it the text confirmation field
    $("#confirmText").append($(".ql-editor").html());

    //this field is for use on the upload and upload edit pages where there is no editor
    //empty the field for file to upload confirmation
    $("confirmFile").empty();
    //take the name of the file to upload and put it in the file to upload confirmation field
    $("confirmFile").append($("input#File").val());

    //this field is for use on the normal create and edit pages where you are working with the editor
    //this is where the information from the editor is put into a format that can be read as part of the submitted form collection
    //#editorContent is a hidden input; because inputs cannot have HTML in them, HTML characters must be escaped
    //the main characters that count as HTML characters are the brackets used for tags
    //therefore, < must be replaced with &lt; and > must be replaced with &gt; before adding the string to #editorContent
    $("#editorContent").val($(".ql-editor").html().replace(/</g, "&lt;").replace(/>/g, "&gt;"));

    //this field is for use on the normal create and edit pages where you are working with the editor
    //this is where the file name (everything before the file extention) is decided for created (not uploaded) writings
    //this file name will be used when the user downloads a piece of writing and is taken from the title
    //because there are certain characters that can't be used in file names, these characters are replaced with ""
    //any kind of white space is replaced with an underscore
    var fileTitle = $("#title").val().replace(/\s/g, "_").replace(/[\~\#\%\&\*\{\}\\\:\<\>\?\/\+\|!=.]/g, "");
    //now it is time to assign the file name to the hidden input so that it can be read as part of the submitted form collection
    //if the fileTitle after getting rid of all the white space and invalid characters is an empty string
    if (fileTitle === "") {
        //put a generic file name like "document" in the #fileName input
        $("#fileName").val("document");
    }
    else //otherwise, if the file name is already a valid one, assign its value to the #fileName input
        $("#fileName").val(fileTitle);

    //empty the validation message field; this is used to put error messages if there is a reason the form can't be submitted
    $("#validationMessage").empty();

    //this field is for use on the normal create and edit pages where you are working with the editor
    //the upload pages do not use a submit button with this ID, so they will not be effected by this
    //if the writing doesn't have a title or an author or text, inform the user and disable the submit button
    if (isEmpty($("#confirmTitle")) || isEmpty($("#confirmProfileID")) || isEmpty($("#confirmText"))) {
        $("#validationMessage").append("Writing cannot be saved without a title, author, and body of text.");
        $("#createWriting").prop("disabled", true);
    }
    else { //otherwise, if the writing is valid, remove the disabled property from the submit button
        $("#createWriting").prop("disabled", false);
    }
}

/*
 * Checks if the value of an HTML element is the same as an empty string
 * by first trimming any white space; returns true if the trimmed string
 * is empty and returns false if the trimmed string is not equal to ""
 * Code from Emil's answer on StackOverflow:
 * https://stackoverflow.com/questions/6813227/how-do-i-check-if-an-html-element-is-empty-using-jquery
 * @param {HTML element} el
 */
function isEmpty(el) {
    return !$.trim(el.html());
}