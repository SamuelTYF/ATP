var table=exporttable;

var rows=table.getElementsByTagName("TR");

console.log(rows)

function ExportTD(td)
{
    if(td.children.length==0)return td.textContent;
    else
    {
        var img=td.getElementsByTagName("IMG")[0];
        var code=img.getAttribute("alt");
        code=code.replaceAll("cfrac","frac");
        code=code.replaceAll("\\displaystyle","");
        code=code.replaceAll("\\lor","\\vee");
        code=code.replaceAll("\\land","\\wedge");
        code=code.replaceAll(" \\","\\");
        return "$$"+code+"$$";
    }
}

var lines=[]
for(var row of rows)
{
    var items=[]
    for(var td of row.getElementsByTagName("TD"))
        items.push(ExportTD(td).replaceAll("\n",""))
    lines.push("| "+items.join(" | ")+" |");
}
console.log(lines.join("\n"))