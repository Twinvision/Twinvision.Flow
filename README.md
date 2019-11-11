# Twinvision.Flow
An HTML builder library to create html using a fluent C# builder pattern. 
See below for some code examples.

### Hello world example
#### Code
```csharp
var builder = new HTMLBuilder();
builder.Document().Body(content: "Hello world!");
System.Diagnostics.Debug.WriteLine(builder.ToString());
```
#### Output
```html
<!DOCTYPE html>
<html>
    <body>Hello world!</body>
</html>
```
### Create div with comments
#### Code
```csharp
var builder = new HTMLBuilder();
builder.Document("en").Body()
       .Comment("A comment for" + Constants.vbCrLf + "a div element")
       .Div(content: "HTML comments test");
System.Diagnostics.Debug.WriteLine(builder.ToString());
```
#### Output
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
### Create component block
#### Code
```csharp
var builder = new HTMLBuilder();
builder.Document().Body()
       .H(1, "Component example")
       .BeginComponent("My Component")
            .Div("Main component element")
                .Child()
                    .AddElement("span", "Extra component content")
                .Parent()
       .EndComponent();
System.Diagnostics.Debug.WriteLine(builder.ToString());
```
#### Result
```html
<!DOCTYPE html>
<html>
    <body>
        <h1>Component example</h1>
        <!-- Begin My Component -->
        <div class="Main component element">
            <span>Extra component content</span>
        </div>
        <!-- End My Component -->
    </body>
</html>
```
