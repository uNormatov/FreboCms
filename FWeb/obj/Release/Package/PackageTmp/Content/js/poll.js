var pollId;
var voteId;

$(document).ready(function(){
  $("#poll").submit(formProcess);
  
  if ($("#poll-results").length > 0 ) {
    animateResults();
  }
  
  if (typeof voteId !== undefined) {
    $("#poll-container").empty();
      $.getJSON("json.ashx?type=<%= SiteConstants.JsonPollVote%>&poll=" + pollid,loadResults);
  }
});

function formProcess(event){
  event.preventDefault();
  
  var id = $("input[@name='poll']:checked").attr("value");
    
  $("#poll-container").fadeOut("slow",function(){
    $(this).empty();
    votedId = id;
    $.getJSON("json.ashx?type=<%= SiteConstants.JsonPollVote%>&poll=" + pollid + "&vote=" + id,loadResults);
    $.cookie('voteid', id, {expires: 365});
    });
}

function animateResults(){
  $("#poll-results div").each(function(){
      var percentage = $(this).next().text();
      $(this).css({width: "0%"}).animate({
				width: percentage}, 'slow');
  });
}


function loadResults(data) {
  var total_votes = 0;
  var percent;
  
  for (choice in data.Choices) {
    total_votes = total_votes+parseInt(choice.VoteCount);
  }
  
  var resultsHtml = "<div id='poll-results'><h3>"+data.Question+"</h3>\n<dl class='graph'>\n";
  for (choice in data) {
    percent = Math.round((parseInt(parseInt(choice.VoteCount))/parseInt(total_votes))*100);
    if (choice.Id !== votedId) {
      resultsHtml = resultsHtml+"<dt class='bar-title'>"+choice.Choice+"</dt><dd class='bar-container'><div id='bar"+choice.Id+"'style='width:0%;'>&nbsp;</div><strong>"+percent+"%</strong></dd>\n";
    } else {
      resultsHtml = resultsHtml+"<dt class='bar-title'>"+choice.Choice+"</dt><dd class='bar-container'><div id='bar"+choice.Id+"'style='width:0%;background-color:#0066cc;'>&nbsp;</div><strong>"+percent+"%</strong></dd>\n";
    }
  }
  
  results_html = results_html+"</dl><p>Total Votes: "+total_votes+"</p></div>\n";
  
  $("#poll-container").append(results_html).fadeIn("slow",function(){
    animateResults();});
}




