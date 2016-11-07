using System;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace LogBot
{
    class Program
    {
        // This is a small 'example' bot, all it does is log a few things to a #log channel
        // Things it logs:  
        //   *  User joining or leaving server.
        //   *  User joining, leaving, or switching a voice channel.
        //   *  User coming online or going offline.
        //  
        // This is only meant to give you an idea of how to do it.  You can add your own stuff.

        // Normally I spread most of my code out between different files, but for the sake of simplicity
        // I've put it all in one file.  

        // This is our DiscordClient.
        public DiscordClient _client;

        // This is YOUR BOT's token.  You will need to change this.
        private string token = "TokenGoesHere";

        // This is the Main method.  Every console app starts with this.
        // We're directing it to use our Start() method instead.
        // This lets us get around an issue ( not being able to use async lambdas ).
        static void Main(string[] args) => new Program().Start();

        public void Start()
        {
            // Here we set our client up,
            // and have it log some useful info to our Console Window.
            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
            });

            // This is where we format/style our Console Window's log messages.
            _client.Log.Message += (s, e) =>
            {
                Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");
            };

            // Let the client know we want to use commands.
            // Set the PrefixChar to ! or whatever character you want. e.g.  !command
            // This also lets you use commands by mentioning the bot, eg: @botname command
            _client.UsingCommands(x =>
            {
                x.AllowMentionPrefix = true;
                x.PrefixChar = '!';
                x.HelpMode = HelpMode.Public;
            });

            // Create our commands here.
            // We'll put them inside of their own method.
            CreateCommands();

            // Log when a user joins the server.
            _client.UserJoined += async (s, e) =>
            {
                // Locate a text channel named "log":  e.g.  #log
                var chan = GetLogChannel(e.Server);

                // Check to make sure the channel exists.
                // If the channel does not exist, return, as there is no place to log the info to.
                if (chan == null) return;

                await chan.SendMessage($"**{e.User.Name}** has joined the server!");
            };

            // Log when a user leaves the server.
            _client.UserLeft += async (s, e) =>
            {
                var chan = GetLogChannel(e.Server);
                if (chan == null) return;
                await chan.SendMessage($"**{e.User.Name}** has left the server!");
            };

            // User Updated for Joining/Leaving/Changing Voice channels.
            _client.UserUpdated += async (s, e) =>
            {
                // Check to see if there is a log channel on the server, before we do anything else.
                var chan = GetLogChannel(e.Server);
                // if it doesn't, return.  That way we don't waste our time.
                if (chan == null) return;


                // Placeholder message, we'll check at the end to see if we have anything to send to our log channel.
                string msg = "";

                // If the voice channel before doesn't exist
                // but the voice channel after does exist
                // Then the user joined a voice channel (was not previously connected to voice)
                if (e.Before.VoiceChannel == null && e.After.VoiceChannel != null)
                    msg = $"**{e.After.Name}** joined the voice channel: **{e.After.VoiceChannel.Name}**";

                // Exactly the opposite of above.  This time the user disconnected from voice.
                else if (e.Before.VoiceChannel != null && e.After.VoiceChannel == null)
                    msg = $"**{e.After.Name}** disconnected from voice, last channel: **{e.Before.VoiceChannel.Name}**";

                // Check to make sure the voice channel exists for both before and after.
                else if (e.Before.VoiceChannel != null && e.After.VoiceChannel != null)
                {
                    // If the voice channel ID before isn't the same as after,
                    // then the user switched voice channels.
                    if (e.Before.VoiceChannel.Id != e.After.VoiceChannel.Id)
                    {
                        msg = $"**{e.After.Name}** has switched from voice channel:  **{e.Before.VoiceChannel.Name}**  to:  **{e.After.VoiceChannel.Name}**";
                    }
                }

                // We can even check if the user has come online or gone offline.
                // If the user was offline before, and no longer offline after, then the user has come online.
                else if (e.Before.Status == UserStatus.Offline && e.After.Status != UserStatus.Offline)
                    msg = $"**{e.After.Name}** has come **online**.";

                // But if the user is offline after when not previously offline, then the status was switched to Offline.
                else if (e.After.Status == UserStatus.Offline && e.Before.Status != UserStatus.Offline)
                    msg = $"**{e.After.Name}** has gone **offline**.";

                // If we changed our placeholder "msg" from "" at any point in time, that means we have a message to send!
                if (msg != "")
                {
                    await chan.SendMessage(msg);
                }
            };
            // Now that we've created our commands and event handlers, we can go ahead and connect the bot.
            // This will always be the last thing we do in this method, as "ExcuteAndWait" blocks the thread.
            // This is so our Console App won't close on us.
            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(token, TokenType.Bot);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Could not connect.  Did you replace the TOKEN in the code?");
                        await Task.Delay(5000);
                    }
                }
            });

        }

        public void CreateCommands()
        {
            var commands = _client.GetService<CommandService>();

            // Create a new command, such as !hello, this one is both : !hello, and !hi
            commands.CreateCommand("hello")
                .Alias(new string[] { "hi" })
                .Description("Make me say hello!")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"Hello {e.User.Mention}");
                });


            // Create a second command.  Lets make it echo whatever you want it to say.
            commands.CreateCommand("say")
                .Alias(new string[] { "echo" })
                .Description("Make me say something!")
                .Parameter("words", ParameterType.Multiple)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(String.Join(" ", e.Args));
                });

            // Alternatively up above, you could use "ParameterType.Unparsed" to get the full uneditted string.


            // If you'd like to add more commands, just copy the way we do it above here..
            // Check out the docs as well:  http://rtd.discord.foxbot.me/en/legacy/features/commands.html
            // and the video: https://www.youtube.com/watch?v=ey8woPqvRaI
            // Some of this stuff is a little dated. 

        }

        // This is to simplify the code above every time we want to find the "log" channel on a server.
        public Channel GetLogChannel(Server s)
        {
            return s.TextChannels.FirstOrDefault(x => x.Name.ToLower() == "log");
        }
    }
}
