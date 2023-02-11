public class AAAAAAAAA
{/*
      
                TODO FIRST
            ------------------

    Make TileClearer immediately change FollowPath's path

    Fix walkability offset issues in pathfinding and grid

    finish (not just for testing script) grid update when you change the walls map (for the walkable bool)

    Could I just do A* based off of tile position? without needing a grid system and all that?
        then just have a walls tilemap, and do the same thing as before with tile penalties?

    maybe make smaller nodes for A*? for more refined paths/ better cornering?
        other fixes needed too.
    fit pathfinding to hero movement , with chasing enemies/treasure when they're within range

    Play Mode
        how active will player be here? 
        maybe you can control enemies, tell them where to go
            plan ambushes
            influence heros' routes
        control other aspects of the dungeon
            secret walls/rooms?
            traps, poison gas, etc
            final boss? control it during final battle? thatd be cool
                have multiple bosses, actually

    Hero movement AI
        may be difficult
        want to aim for treasure, gather treasure, then go toward next (for now)
            maybe aim for enemies too if they're nearby?
        have their overall aim be the final boss/big treasure or whatever
            might help with pathfinding



                   TODO
               ------------

    Dungeon Building system
        Miners to dig new rooms, maybe they can fight too (weakly)
        place traps/enemies individually? or premade rooms? not sure yet
        have a "build mode" before gameplay
            get mouseposition, round to int, click menu to choose item to place, click on tile to place item
            keep it grid based for now to avoid overlapping

    Save System
        scriptable object that holds a huge dictionary with grid value as the key, 
            and whatever is placed there as the value? no, dictionary not serializable. maybe use a workaround? two lists?
        holds "blank" for empty spaces that have been dug out, no entry/delete entry for undug tiles (dirt)?
        another SO for boss stats
        another for your resources?
        another for every possible statistic: time played, # of skeletons died, heroes killed, everything
            maybe get ridiculous, have different levels of specificity. you can choose how deep to look in the menu
        have an inventory one too, for rare artifacts or whatever. (dont want too many items in game)

    Have a final target for heroes. then have a detect enemy/treasure radius so they go toward them whenever they're
        within the radius, then back to the target
            maybe have a "goingTowardsEnd" bool that switches off when hero senses another enemy/treasure nearby?


                GAME OVERVIEW
            ---------------------

    Build a dungeon, fill with traps and enemies

    collect loot/money/magic from slain heroes
        also mine for gold and metals/rocks to build with

    if a hero kills your boss, then you lose (they get) a bunch of loot/money
        have your final boss be upgradeable/levelable/equipable

    use loot from heroes to build more rooms
        collect enough magic potions to develop new magical defenses?
        collect heros' souls when they die, then use those as "currency" to build more bad guys (goblins, skeletons, etc)

    have digger grunts to dig out new rooms, then build specific stuff in them
        maybe do specific premade rooms with certain effects?

    treat treasure just like an enemy that doesnt move or attack and has good loot, at least for now
        "health" is the amount of time spent gathering the treasure

    traps n stuff
        illusion of a powerful being that scares off weaker adventurers
            they could leave loot when they run
        use chests/treasure as bait for heroes
            have traps and ambushes waiting

    encourage maze building so heroes have to backtrack and fight more/have another chance to hit traps
    
    in the beginning, you're just a necromancer and a goblin in a cave, fighting off weak heroes from 
        the local village. You were chased out of town for being a necromancer or something
    You dig and build and get stronger, as the town grows and produces better heroes
        -maybe you can win by destroying the village? or should it be endless? or have both options
            maybe after you "win" you can move to a new area with a bigger city and better heroes,
                kinda like difficulty levels? better loot and all that with it?

    have a reputation stat, which is essentially exp. higher reputation brings stronger heroes.
        reputation wont have inherent quality like leveling up,
            it'll just bring better heroes, which will in turn level you up faster. maybe
        show it as a bar, no number amount. so you will "level up". maybe.
    "level up" the dungeon through reputation
    brings stronger heroes with better loot (and stronger souls)
    Other legendary enemies will hear of you and join your dungeon

    Maybe you are a wizard/necromancer. You steal heros' souls and use that "energy" to raise skeletons, 
        make goblins or whatever.
        You can do things to help your dungeon/minions while heroes are invading
            cause a tunnel collapse to delay heroes
            magic spells
            heal/buff bosses?

    Maybe have no build mode, just build in real time, while heroes are attacking

    heroes chose which way to go randomly when they have a choice (but don't go back unless they have to)
        pathfind to a series of waypoints?

    carve out each tile individually (with miners) so you can have custom shaped rooms
        might make for bad gameplay, require fine tuning?

    How to avoid players just making a straight shot dungeon lined with defenses?
        how to incentivize building two dimensionally?
            maybe have rock that's too hard to mine through
                it'd be like premade maps kinda
            also, make it so you get overwhelmed if you have just one path
                need multiple routes with traps and such to successfully defend

    have different mineral deposits you can mine, some rock is too hard to carve through

    You control the area outside of the entrance too. you can get wood from the trees
        can also set up defenses outside, make it look cool

    _____________________________________________________________________
    resource                                    thing built with resource
    --------                                    -------------------------
    hero souls                                  enemies, maybe strengthen bosses?

    money (from dead heroes)                    buy stuff?

    iron (from equipment)                       traps, equipment, all sorts of stuff

    better metals                               better versions of stuff
        (from higher lvl heroes)      

    random special things                       unique special stuff
        (magic gems,artifacts, etc)

    .






















































*/
}