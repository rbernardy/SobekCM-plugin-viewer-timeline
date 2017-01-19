function fixFullsize()
{
	$("iframe").parent().css('max-width','100%');

	$("iframe").parent().parent().parent().remove();

	if ($("iframe").length)
	{
		$("div#primary > h2").remove();
		$("div#primary").css("padding","0px").css("width","725px").css("margin","0px");
	}	
}

function timelineScreenSize() 
{
	console.log("timelineScreenSize was called...");
			
	width=$("div#primary").width();
	console.log("width=[" + width + "].");

	dims=getViewportDimensions();
	console.log("width=" + dims[0] + ", height=" + dims[1] + ".");

	if (width==725)
	{
		console.log("was original size, switching to full screen.");
		$("div#primary").css("width","100%").css("height","100%").css("z-index","999").css("position","absolute").css("left","0px").css("top","0px");
		src=$("iframe").attr("src") + "&width=" + dims[0] + "&height=" + (dims[1]-50);
		console.log("src=["+src+"].");
		$("iframe").width(dims[0]).height((dims[1]-50)).attr("src",src);
		$("iframe").attr("src",src + "&random=" + Math.random());
		$("#tlbutton").text("Return to exhibit page");
	}
	else
	{
		console.log("was full screen, returning to original size.");
		$("div#primary").width(725).css("height","100%").css("z-index","0").css("position","relative");
		$("iframe").width(725).height(650);
		src=src.substring(0,src.indexOf("&width"));
		$("iframe").attr("src",src + "&random=" + Math.random());
		$("#tlbutton").text("Timeline to Full Screen Size");
	}
}

function getViewportDimensions()
{
	var viewportwidth;
	var viewportheight;
  
	if (typeof window.innerWidth != 'undefined')
	{
		viewportwidth = window.innerWidth,	
		viewportheight = window.innerHeight;
	}
	else if (typeof document.documentElement != 'undefined' && typeof document.documentElement.clientWidth !='undefined' && document.documentElement.clientWidth != 0)
	{
		viewportwidth = document.documentElement.clientWidth,
		viewportheight = document.documentElement.clientHeight;	
	}
	else
	{
		viewportwidth = document.getElementsByTagName('body')[0].clientWidth,
		viewportheight = document.getElementsByTagName('body')[0].clientHeight;
	}

	var dimensions = new Array(2,2);
	dimensions[0,0]=viewportwidth;
	dimensions[0,1]=viewportheight;

	return dimensions;
}