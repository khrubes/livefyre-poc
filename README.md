# livefyre-poc

This is a ASP.NET web app template generated by Visual Studio I've used to demonstrate the issue we are experiencing with LiveFyre.
The only files to look at are:

1. [Views/Shared/_Layout.cshtml](https://github.com/khrubes/livefyre-poc/blob/master/livefyre-test/livefyre-test/Views/Shared/_Layout.cshtml) : 
where Livefyre.js is included 



2. [Controllers/HomeController.cs](https://github.com/khrubes/livefyre-poc/blob/master/livefyre-test/livefyre-test/Controllers/HomeController.cs) :
where I followed http://answers.livefyre.com/developers/reference/generating-livefyre-tokens/ to generate Livefyre tokens in C#
step 4 in this tutorial may be of note, JWT.JsonWebToken is not a known field according to the most recent JWT package



3. [Views/Home/Index.cshtml](https://github.com/khrubes/livefyre-poc/blob/master/livefyre-test/livefyre-test/Views/Home/Index.cshtml) :
where we are attempting to initialize the comments plugin with the generated serverside tokens


