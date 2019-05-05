
$(document).ready(

    function () {
	$("#wishlist").sortable({
	    axis: "y",
	    containment: "parent",
	    update: onGiftPriorityChange
	});
    }

);

function onGiftPriorityChange(event, ui)
{
    $.ajax({
	url: "Account",
	method: "POST",
	data:
	{
	    action: "change_priority",
	    gift_id: ui.item.attr("id"),
	    gift_new_priority: ui.item.index()
	}
    });
}

function removeGift(id)
{
    $.ajax({
	url: "Account",
	method: "POST",
	data:
	{
	    action: "remove_gift",
	    gift_id: id
	}
    });

    $("#" + id).fadeOut(500, function () { $("#" + id).remove(); });
}