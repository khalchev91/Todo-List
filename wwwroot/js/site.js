﻿// Write your JavaScript code.

$(document).ready(function () {
   $('.done-checkbox').on('click',function (e) {
       markCompleted(e.target);
   }) ;
});

function markCompleted(checkbox) {
    checkbox.disabled = true;
    
    var row = checkbox.closest('tr');
    $(row).addClass('done');
    
    var form = checkbox.closest('form');
    form.submit(); 
}