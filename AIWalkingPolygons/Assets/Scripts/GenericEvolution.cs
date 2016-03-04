﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GenericEvolution
{
    public double mutatioinProb = 0.1;
    public double crossoverProb = 0.9;
    private System.Random rand = new System.Random();

    List<Creature> CreateNextGen(List<Creature> creatures)
    {
        List<Creature> nextGen = new List<Creature>();
        List<double> roulette = getRoulette(creatures);

        for (int i = 0; i < creatures.Count / 2; i++)
        {
            int indexP1 = getIndexFromRoulette(roulette);
            int indexP2 = getIndexFromRoulette(roulette);
            Creature child1 = creatures[indexP1];
            Creature child2 = creatures[indexP2];

            if (rand.NextDouble() < crossoverProb)
            {
                List<Creature> parents = new List<Creature>();
                parents.Add(child1);
                parents.Add(child2);

                List<Creature> children = crossover(parents);
                child1 = children[0];
                child2 = children[1];


                if (rand.NextDouble() < mutatioinProb)
                {
                    child1 = mutate(child1);
                }
                if (rand.NextDouble() < mutatioinProb)
                {
                    child2 = mutate(child2);
                }
            }

            nextGen.Add(child1);
            nextGen.Add(child2);
        }

        return nextGen;
    }

    List<Creature> crossover(List<Creature> creatures)
    {
        List<MovementNode> sequence1 = creatures[0].Sequence;
        List<MovementNode> sequence2 = creatures[1].Sequence;

        int randInt = rand.Next() % 3;
        MovementNode m1 = sequence1[randInt];
        MovementNode m2 = sequence2[randInt];

        sequence1[randInt] = m2;
        sequence2[randInt] = m1;

        creatures[0].Sequence = sequence1;
        creatures[1].Sequence = sequence2;
        
        return creatures;
    }

    Creature mutate(Creature creature)
    {
        List<MovementNode> sequence = creature.Sequence;
        foreach(MovementNode mn in sequence)
        {
            mn.Force = mn.Force * (1 + (float)rand.NextDouble() - 0.5F);
            mn.Delay = mn.Delay * (1 + (float)rand.NextDouble() - 0.5F);
        }

        List<int> segment = shuffle();
        for(int i=0; i<segment.Count; i++)
        {
            sequence[i].Segment = segment[i];
        }
        
        return creature;
    }


    private double evaluateCreature(Creature creature)
    {
        EvalFunction ef = new EvalFunction();
        return ef.evalFunction(creature);
    }


    private List<int> shuffle()
    {
        List<int> list = new List<int>();
        list.Add(0);
        list.Add(1);
        list.Add(2);

        List<int> shuffled = new List<int>();

        int index = rand.Next() % 3;
        shuffled.Add(list[index]);
        list[index] = list[2];

        index = rand.Next() % 2;
        shuffled.Add(list[index]);
        list[index] = list[1];

        shuffled.Add(list[0]);

        return shuffled;
    }

    private int getIndexFromRoulette(List<double> roulette)
    {
        double rnum = rand.NextDouble();
        for (int i = 0; i < roulette.Count; i++)
        {
            if (rnum <= roulette[i])
                return i;
        }

        return roulette.Count - 1;
    }

    private List<double> getRoulette(List<Creature> creatures)
    {
        List<double> roulette = new List<double>();
        foreach (Creature creature in creatures)
        {
            roulette.Add(evaluateCreature(creature));
        }

        return comulateRoulette(normalizeRoulette(roulette));
    }

    private List<double> normalizeRoulette(List<double> roulette)
    {
        List<double> normalized = new List<double>();

        double sum = 0;
        foreach (double d in roulette)
        {
            sum += d;
        }

        foreach (double f in roulette)
        {
            normalized.Add(f / sum);
        }

        return normalized;
    }

    private List<double> comulateRoulette(List<double> roulette)
    {
        List<double> comulated = new List<double>();
        comulated.Add(roulette[0]);
        for (int i = 1; i < roulette.Count; i++)
        {
            comulated.Add(comulated[i - 1] + roulette[i]);
        }
        return comulated;
    }
}
