﻿using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;
using System.Diagnostics;
using System.IO;

namespace SomeDiscordBotThing
{
    public class ExampleVoiceCommands : BaseCommandModule
    {
        [Command("join"), Description("Joins a voice channel.")]
        public async Task Join(CommandContext ctx, DiscordChannel chn = null)
        {
            try
            {
                // check whether VNext is enabled
                var vnext = ctx.Client.GetVoiceNext();
                if (vnext == null)
                {
                    // not enabled
                    await ctx.RespondAsync("VNext is not enabled or configured.");
                    return;
                }
                Console.WriteLine("hi1");
                // check whether we aren't already connected
                var vnc = vnext.GetConnection(ctx.Guild);
                if (vnc != null)
                {
                    // already connected
                    await ctx.RespondAsync("Already connected in this guild.");
                    return;
                }
                Console.WriteLine("Hi");

                // get member's voice state
                var vstat = ctx.Member.VoiceState;
                if (vstat.Channel == null && chn == null)
                {
                    // they did not specify a channel and are not in one
                    await ctx.RespondAsync("You are not in a voice channel.");
                    return;
                }
                Console.WriteLine("hi2");
                // channel not specified, use user's
                if (chn == null)
                    chn = vstat.Channel;

                // connect
                Console.WriteLine("Hi3");
                vnc = await vnext.ConnectAsync(chn);
                await ctx.RespondAsync($"Connected to `{chn.Name}`");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [Command("leave"), Description("Leaves a voice channel.")]
        public async Task Leave(CommandContext ctx)
        {
            // check whether VNext is enabled
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we are connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
            {
                // not connected
                await ctx.RespondAsync("Not connected in this guild.");
                return;
            }

            // disconnect
            vnc.Disconnect();
            await ctx.RespondAsync("Disconnected");
        }

        [Command("play"), Description("Plays an audio file.")]
        public async Task Play(CommandContext ctx, [RemainingText, Description("Full path to the file to play.")] string filename)
        {
            // check whether VNext is enabled
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we aren't already connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
            {
                // already connected
                await ctx.RespondAsync("Not connected in this guild.");
                return;
            }

            // check if file exists
            if (!File.Exists(filename))
            {
                // file does not exist
                await ctx.RespondAsync($"File `{filename}` does not exist.");
                return;
            }

            // wait for current playback to finish
            while (vnc.IsPlaying)
                await vnc.WaitForPlaybackFinishAsync();

            // play
            Exception exc = null;
            await ctx.Message.RespondAsync($"Playing `{filename}`");

            try
            {
                await vnc.SendSpeakingAsync(true);

                var psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = $@"-i ""{filename}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                var ffmpeg = Process.Start(psi);
                var ffout = ffmpeg.StandardOutput.BaseStream;

                var txStream = vnc.GetTransmitSink();
                await ffout.CopyToAsync(txStream);
                await txStream.FlushAsync();
                await vnc.WaitForPlaybackFinishAsync();
            }
            catch (Exception ex) { exc = ex; }
            finally
            {
                await vnc.SendSpeakingAsync(false);
                await ctx.Message.RespondAsync($"Finished playing `{filename}`");
            }

            if (exc != null)
                await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        }
    }
}

