/**
 * @name SixMansListener
 * @author wildcherry
 * @authorId 200336755235356672
 * @version 0.1.0
 * @description Listens for !q and !leave messages to replicate 6Mans queues externally. Forked from DevilBro's ChatFilter 3.5.5.
 */

module.exports = (_ => {
	const config = {
		"info": {
			"name": "SixMansListener",
			"author": "wildcherry",
			"version": "0.1.0",
			"description": "Listens for !q and !leave messages to replicate 6Mans queues externally. Forked from DevilBro's ChatFilter 3.5.5."
		},
		"changeLog": {}
		
	};

	return (window.Lightcord && !Node.prototype.isPrototypeOf(window.Lightcord) || window.LightCord && !Node.prototype.isPrototypeOf(window.LightCord) || window.Astra && !Node.prototype.isPrototypeOf(window.Astra)) ? class {
		getName () {return config.info.name;}
		getAuthor () {return config.info.author;}
		getVersion () {return config.info.version;}
		getDescription () {return "Do not use LightCord!";}
		load () {BdApi.alert("Attention!", "By using LightCord you are risking your Discord Account, due to using a 3rd Party Client. Switch to an official Discord Client (https://discord.com/) with the proper BD Injection (https://betterdiscord.app/)");}
		start() {}
		stop() {}
	} : !window.BDFDB_Global || (!window.BDFDB_Global.loaded && !window.BDFDB_Global.started) ? class {
		getName () {return config.info.name;}
		getAuthor () {return config.info.author;}
		getVersion () {return config.info.version;}
		getDescription () {return `The Library Plugin needed for ${config.info.name} is missing. Open the Plugin Settings to download it. \n\n${config.info.description}`;}
		
		downloadLibrary () {
			require("request").get("https://mwittrien.github.io/BetterDiscordAddons/Library/0BDFDB.plugin.js", (e, r, b) => {
				if (!e && b && r.statusCode == 200) require("fs").writeFile(require("path").join(BdApi.Plugins.folder, "0BDFDB.plugin.js"), b, _ => BdApi.showToast("Finished downloading BDFDB Library", {type: "success"}));
				else BdApi.alert("Error", "Could not download BDFDB Library Plugin. Try again later or download it manually from GitHub: https://mwittrien.github.io/downloader/?library");
			});
		} 
		
		load () {
			if (!window.BDFDB_Global || !Array.isArray(window.BDFDB_Global.pluginQueue)) window.BDFDB_Global = Object.assign({}, window.BDFDB_Global, {pluginQueue: []});
			if (!window.BDFDB_Global.downloadModal) {
				window.BDFDB_Global.downloadModal = true;
				BdApi.showConfirmationModal("Library Missing", `The Library Plugin needed for ${config.info.name} is missing. Please click "Download Now" to install it.`, {
					confirmText: "Download Now",
					cancelText: "Cancel",
					onCancel: _ => {delete window.BDFDB_Global.downloadModal;},
					onConfirm: _ => {
						delete window.BDFDB_Global.downloadModal;
						this.downloadLibrary();
					}
				});
			}
			if (!window.BDFDB_Global.pluginQueue.includes(config.info.name)) window.BDFDB_Global.pluginQueue.push(config.info.name);
		}
		start () {this.load();}
		stop () {}
		getSettingsPanel () {
			let template = document.createElement("template");
			template.innerHTML = `<div style="color: var(--header-primary); font-size: 16px; font-weight: 300; white-space: pre; line-height: 22px;">The Library Plugin needed for ${config.info.name} is missing.\nPlease click <a style="font-weight: 500;">Download Now</a> to install it.</div>`;
			template.content.firstElementChild.querySelector("a").addEventListener("click", this.downloadLibrary);
			return template.content.firstElementChild;
		}
	} : (([Plugin, BDFDB]) => {
		var oldBlockedMessages, oldCensoredMessages, words;
		
		return class SixMansListener extends Plugin {
			onLoad () {
				this.defaults = {
					replaces: {
						blocked: 				{value: "~~BLOCKED~~",		description: "Default Replacement Value for blocked Messages: "},
						censored:				{value: "$!%&%!&",			description: "Default Replacement Value for censored Messages: "}
					},
					general: {
						addContextMenu:			{value: true,				description: "Add a Context Menu Entry to faster add new blocked/censored Words"},
						targetMessages:			{value: true,				description: "Check Messages for blocked/censored Words"},
						targetStatuses:			{value: true,				description: "Check Custom Statuses for blocked/censored Words"},
						targetOwn:				{value: true, 				description: "Filter/Block your own Messages/Custom Status"}
					}
				};
			
				this.patchedModules = {
					before: {
						Message: "default",
						MessageContent: "type",
						UserInfo: "default",
						MemberListItem: "render",
						PrivateChannel: "render"
					},
					after: {
						Messages: "type",
						MessageContent: "type",
						Embed: "render"
					}
				};
			}
			
			onStart () {
				words = BDFDB.DataUtils.load(this, "words");
				for (let rType in this.defaults.replaces) if (!BDFDB.ObjectUtils.is(words[rType])) words[rType] = {};
				this.forceUpdateAll();
			}
			
			onStop () {
				this.forceUpdateAll();
			}

			
			forceUpdateAll () {					
				oldBlockedMessages = {};
				oldCensoredMessages = {};
				
				BDFDB.PatchUtils.forceAllUpdates(this);
				BDFDB.MessageUtils.rerenderAll();
			}

			processMessages (e) {
				if (this.settings.general.targetMessages) {
					e.returnvalue.props.children.props.channelStream = [].concat(e.returnvalue.props.children.props.channelStream);
					for (let i in e.returnvalue.props.children.props.channelStream) {
						let message = e.returnvalue.props.children.props.channelStream[i].content;
						if (message) {
							//console.log(this.isPlayerTargetMessage(message));
							// if (BDFDB.ArrayUtils.is(message.attachments)) this.checkMessage(e.returnvalue.props.children.props.channelStream[i], message);
							// else if (BDFDB.ArrayUtils.is(message)) for (let j in message) {
								// let childMessage = message[j].content;
								// if (childMessage && BDFDB.ArrayUtils.is(childMessage.attachments)) this.checkMessage(message[j], childMessage);
							// }
						}
					}
				}
			}

			isInRankBChat(message){
				return message.channel_id == "716744866876620920";
			}

			isPlayerTargetMessage(message){
				return !message.bot && (message.content == "!q" || message.content == "!leave");
			}

			isBotTargetMessage(message){

			}
		};
	})(window.BDFDB_Global.PluginUtils.buildPlugin(config));
})();