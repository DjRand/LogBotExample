# LogBotExample
an example LogBot using Discord.Net 0.9.6

I wrote this quickly, though everything should work.

## Before you begin:
###Getting your bot on your server:
*  Please visit the [Discord](https://discordapp.com/) website.
*  Make an account if you do not already have one.
*  Login to your account.
*  Go to the [My Applications](https://discordapp.com/developers/applications/me) page.
*  Click "New Application"
*  "App Name" this will be the name of your bot, put whatever you want.
*  "App Description" whatever you want.
*  "App Icon" select a picture for your bot's Avatar.
*  Click "Create Application"
*  Then click "Create a Bot User"
*  Where it says "APP BOT USER" just below that it says "Token - click to reveal"
*  Click that.  Save this token somewhere.  You will need it.
*  Where it says "APP DETAILS" above all that, you will see "Client ID:"
*  Copy the "Client ID" section, should look like: 23429304234203
*  https://discordapp.com/oauth2/authorize?client_id=CLIENT_ID&scope=bot
*  Put this link into your browser, and replace "CLIENT_ID" with the numbers you just copied, then press enter.
*  If you own a server, you will be able to invite your bot to your server this way.
*  Now that your bot is on your server, you need to install Visual Studio 2015 Community AND Update 3, along with the .NET Core Plugin. Make sure you get all of this.


## What you need:
*  [Visual Studio 2015](https://www.visualstudio.com/downloads/download-visual-studio-vs)  
*  [Visual Studio 2015 Update 3](https://www.microsoft.com/net/core#windows)  
*  [.NET Core Plugin](https://www.microsoft.com/net/core#windows)  

Once you have these all set up, you should be able to compile this program. \o/  
Windows 7 might have some issues.  Either be on Windows 8 or Windows 10 and you should be good.
  
  
  
### In Program.cs:
*  You'll need to put the TOKEN you copied earlier there.
*  You'll be able to compile the program into an .exe from there.
  
  
  
 
#### This is written with the Discord.Net 0.9.6 lib.
*  [You don't need to download this, but you can find that here.](https://github.com/RogueException/Discord.Net/tree/master)

#### You can find docs for 0.9.* over on Foxbot's site:
*  [Docs in case you want to modify anything.](http://rtd.discord.foxbot.me/en/legacy/features/commands.html)
