# Twinvision.Flow
Fluent HTML builder library

## Hello world example
### Code
```csharp
var builder = new HTMLBuilder();
builder.Document().Body(content: "Hello world!");
System.Diagnostics.Debug.WriteLine(builder.ToString());
```
### Output
```html
<!DOCTYPE html>
<html>
    <body>Hello world!</body>
</html>
```
## Create div with multi line comments
### Code
```csharp
var builder = new HTMLBuilder();
builder.Document("en").Body()
       .Comment("A comment for" + Constants.vbCrLf + "a div element")
       .Div(content: "HTML comments test");
System.Diagnostics.Debug.WriteLine(builder.ToString());
```
### Output
```html
<!DOCTYPE html>
<html lang="en">
    <body>
        <!--
            A comment for
            a div element
        -->
        <div>HTML comments test</div>
    </body>
</html>
```
## Create component blocks with optional comments 
