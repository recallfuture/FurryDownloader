# Furry批量下载器
用于根据作者名批量下载[furaffinity](http://www.furaffinity.net)网站的图片。

## 使用方法
打开后填入作者名，选择图片存放位置和图集后即可开始下载。默认为下载此作者的所有图片。

部分作者的内容需登陆后才可查看，这时需要将登陆后的网站cookie填入‘自动登录’栏，然后再下载即可。

各浏览器获取cookie的方式请自行百度

## cookie格式
键值对形式，key=value;key=value;

必须有的三个键:a,b,__cfduid（缺一不可，其他的键则可以省略,分号不可以省略）

例：a=asdasdasdasdasdasdasd;b=3bb90133-4fed-405d-bf0d-4391baa8ed20; __cfduid=dee5d6da0b534f590c423cc6a50c753e51486115641;

其他使用说明在应用内有提示。

-----------

引用代码：[HttpHelper.cs](http://www.sufeinet.com/thread-3-1-1.html)
