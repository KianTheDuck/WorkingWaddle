using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SomeDiscordBotThing.commands
{
    public class commandthingy : BaseCommandModule
    {
        [Command("say")]
        public async Task SayAsync(CommandContext ctx, [RemainingText] string echo)
        {
            if (echo == "nword")
            {
                await ctx.Message.DeleteAsync();
                await ctx.Channel.SendMessageAsync("No racism pls :)");
            }
            else
            {
                await ctx.Message.DeleteAsync();
                await ctx.Channel.SendMessageAsync(echo);
            }

        }
        [Command("bing")]
        public async Task BongAsync(CommandContext ctx)
        {

            await ctx.Channel.SendMessageAsync("Bong");

        }
        [Command("penis")]
        public async Task penisAsync(CommandContext ctx, DiscordMember member)
        {

            await ctx.Message.DeleteAsync();
            var random = new Random();
            var penlength = random.Next(1, 50);
            var penstring = "";
            for (int i = 0; i < penlength; i++)
            {
                penstring += "=";
            }
            if (member.Id == 770296717433241610)
            {
                await ctx.Channel.SendMessageAsync("He is too powerful");
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"{member.Mention}'s penis: 8{penstring}D");
            }
        }
        [Command("penis")]
        public async Task penisAsync(CommandContext ctx)
        {

            await ctx.Message.DeleteAsync();
            var random = new Random();
            var penlength = random.Next(1, 50);
            var penstring = "";
            for (int i = 0; i < penlength; i++)
            {
                penstring += "=";
            }

            await ctx.Channel.SendMessageAsync($"{ctx.Member.Mention}'s penis: 8{penstring}D");

        }
        [Command("cheese")]
        public async Task cheeseAsync(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("https://tenor.com/view/cheese-gif-21664094");
        }
        [Command("quack")]
        public async Task quackAsync(CommandContext ctx, [RemainingText] string role)
        {
            try
            {
                var guildroles = ctx.Guild.Roles.Values;
                var guildrole = ctx.Guild.Roles;
                foreach (var r in guildroles)
                {
                    Console.WriteLine(r.Name);

                    if (r.Name == role)
                    {
                        //DiscordRole quackrole;
                        //if (!guildrole.ContainsKey(r.Id))
                        //{

                        //    quackrole = await ctx.Guild.CreateRoleAsync("quack");
                        //    await ctx.Member.GrantRoleAsync(quackrole);
                        var rolee = ctx.Guild.GetRole(r.Id);
                        await ctx.Member.GrantRoleAsync(rolee);
                        await ctx.Channel.SendMessageAsync("https://tenor.com/view/happy-duck-pet-cute-gif-12986190");
                        //}
                        //else
                        //{
                        //    await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(r.Id));
                        //    await ctx.Channel.SendMessageAsync("https://tenor.com/view/happy-duck-pet-cute-gif-12986190");
                        //}


                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
        [Command("banCameron")]
        public async Task banCameronAsync(CommandContext ctx)
        {

            await ctx.Guild.BanMemberAsync(447161471072337949);
            await ctx.Channel.SendMessageAsync("Lol you just banned cameron");

        }
        [Command("unbanIdiot")]
        public async Task unbanCameronAsync(CommandContext ctx)
        {

            await ctx.Guild.UnbanMemberAsync(447161471072337949);
            await ctx.Channel.SendMessageAsync("wowwww you unbanned him");

        }
        [Command("nick"), Description("Gives someone a new nickname.")]
        public async Task ChangeNickname(CommandContext ctx, [Description("Member to change the nickname for.")] DiscordMember member, [RemainingText, Description("The nickname to give to that user.")] string new_nickname)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await member.ModifyAsync(x =>
                {
                    x.Nickname = new_nickname;
                    x.AuditLogReason = $"Changed by {ctx.User.Username} ({ctx.User.Id}).";
                });

                // let's make a simple response.
                var emoji = DiscordEmoji.FromName(ctx.Client, ":+1:");

                // and respond with it.
                await ctx.RespondAsync(emoji);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker know
                var emoji = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await ctx.RespondAsync(emoji);
            }

        }
        [Command("fakeban")]
        public async Task fakebanAsync(CommandContext ctx, DiscordMember member, [RemainingText]string reason)
        {
            await ctx.Channel.TriggerTypingAsync();
            await ctx.Message.DeleteAsync();
            var embed = new DiscordEmbedBuilder();
            embed.WithTitle("Ban");
            embed.WithDescription("User Banned");
            embed.AddField("Banned User", member.Mention, true);
            embed.AddField("Responsible Moderator", ctx.Member.Mention, true);
            embed.AddField("Reason", reason, false);
            embed.WithColor(DiscordColor.Blue);
            embed.WithThumbnail(member.AvatarUrl);
            var timeuserbanned = DateTime.Now;
            embed.WithFooter($"{timeuserbanned}| Channel ID:{ ctx.Channel.Id}|User ID:{member.Id}");

            await ctx.Channel.SendMessageAsync(embed.Build());
        }
        [Command("hehe")]
        public async Task heheAsync(CommandContext ctx, DiscordMember member)
        {
            await ctx.Channel.TriggerTypingAsync();
            await ctx.Message.DeleteAsync();
            await member.CreateDmChannelAsync();
            await member.SendMessageAsync($"{member.Mention} Hiu");
        }
    }


}
