var scrollSpeed : float = 0.5;
var variableSpeed : boolean = false;
var scrollVert : boolean = true;
var scrollHorz : boolean = false;
private var offset : float;
	
function Start()
	{
	if(variableSpeed == true)
		scrollSpeed = scrollSpeed + Random.Range(-0.1f, 0.1f);
	}	
	
function Update()
	{
	offset = Time.time * scrollSpeed;
	if(scrollHorz)
		GetComponent.<Renderer>().material.mainTextureOffset = Vector2(offset, 0);
	if(scrollVert)
		GetComponent.<Renderer>().material.mainTextureOffset = Vector2(0, offset);
	}