local player = {}
local monster = require("module.monster.monster")
local GameObject = CS.UnityEngine.GameObject
local Animator = CS.UnityEngine.Animator
local Vector3 = CS.UnityEngine.Vector3
player.hurtNum = 5
player.CurrentEnemy = {}
player.name = "player1"
local module = require("module.module")
local CodeType = require("module.handler.codeType")
local animator = require("module.player.playerAnimator")
local event = require("module.event.event")
function player:New(o)
	local instance = o or {}
	setmetatable(instance, self)
	self.__index = self
	instance:Init()
	return instance
end
function player:Init()
	print("玩家初始化！！！！！！！！！！！！！！！！！！！")
	
	-- self.player = GameObject.Find("RPG_Player").transform:Find("RPG-Character").gameObject
	self.player = GameObject.Find("RPG-C").transform:Find("RPG"):Find("RPG-Character").gameObject

	self.handler = require("module.player.playerHandler")
	self.cs_animator = self.player:GetComponent(typeof(Animator))
	self.animator = animator:New();
	self.animator:Init(self.player, self.cs_animator, self)
	self.handler:Init()
	event.tendKeyDown:register(self.UpdateTend, self)
end


function player:Attack(delta)
	print("player 1 Attack!!!!!!!  " ,delta)

	for _, value in pairs(self.CurrentEnemy) do
		value:Hurt(self.hurtNum)
	end

	module.event.playerHurt:trigger("哇哇哇哇哇哇哇")
end
function player:UpdateTend(tend)
	print("?????");
	self.cs_animator:SetFloat("Blend", tend.x);
	self.cs_animator:SetFloat("Blend2", tend.y);
	self.cs_animator:SetLayerWeight(0, 0)
	
	self.cs_animator:SetLayerWeight(1, tend.sqrMagnitude)
end
function player:Update(delta)
	self.animator:Update(delta)
end










return player