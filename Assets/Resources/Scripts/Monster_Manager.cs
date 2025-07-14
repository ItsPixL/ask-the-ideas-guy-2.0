using UnityEngine;
using Sprites;
using LevelManager;
using System.Collections.Generic;

namespace MonsterManager
{
    public abstract class Monster : Thing
    {
        public int health;
        public int damage;
        public int movement;
        public int attackRange;
        public int sightRange;
    }

    public class Brute : Monster
    {
        public Brute(Sprite monsterSprite, (int, int) startingCoords, List<int> basicStats)
        {
            thingSprite = monsterSprite;
            coordPos = startingCoords;
            health = basicStats[0];
            damage = basicStats[1];
            movement = basicStats[2];
            attackRange = basicStats[3];
            sightRange = basicStats[4];
        }

    }

    public class MonsterSpawner
    {
        public Sprite spawnerSprite;
        public (int, int) coordPos;
        public List<(int, int)> possibleSpawnPos;
        public List<int> spawnedBasicStats = new List<int>();
        public string monsterType;
        public int turnCooldown;
        public int turnsLeft;
        public bool activated;
        public Level levelObject;

        public MonsterSpawner(Sprite spawnerSprite, (int, int) coordPos, string monsterType, List<int> spawnedBasicStats)
        {
            this.spawnerSprite = spawnerSprite;
            this.coordPos = coordPos;
            int coordX = coordPos.Item1;
            int coordY = coordPos.Item2;
            possibleSpawnPos = new List<(int, int)> { (coordX - 1, coordY), (coordX + 1, coordY), (coordX, coordY - 1), (coordX, coordY + 1) };
            this.monsterType = monsterType;
            this.spawnedBasicStats = spawnedBasicStats;
        }

        public void spawnMonster((int, int) gridCoords)
        {
            if (monsterType == "brute")
            {
                Brute newBrute = new Brute(SpriteLibrary.bruteSprite, gridCoords, spawnedBasicStats);
                levelObject.addThingOnField(newBrute, gridCoords);
            }
        }

        public void trackMonsterSpawning()
        {
            if (activated)
            {
                if (turnsLeft >= 1)
                {
                    turnsLeft -= 1;
                }
                if (turnsLeft == 0)
                {
                    List<(int, int)> validSpawnPos = new List<(int, int)>();
                    foreach (var item in possibleSpawnPos)
                    {
                        if (levelObject.isInField(item) && !levelObject.isOccupied(item))
                        {
                            validSpawnPos.Add(item);
                        }
                    }
                    if (validSpawnPos.Count > 0)
                    {
                        int choiceIdx = Random.Range(0, validSpawnPos.Count);
                        (int, int) chosenCoords = validSpawnPos[choiceIdx];
                        spawnMonster(chosenCoords);
                    }
                }
            }
        }
    }
}