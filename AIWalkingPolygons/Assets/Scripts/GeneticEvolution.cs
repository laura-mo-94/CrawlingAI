using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneticEvolution
{
    public double mutatioinProb = 0.3;
    public double crossoverProb = 0.9;
    private System.Random rand = new System.Random();

    public List<Creature> CreateNextGen(List<Creature> creatures)
    {
        //return creatures;
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

        int bestIndex = findBest(nextGen);
        Creature best = nextGen[bestIndex];
        nextGen[bestIndex] = nextGen[0];
        nextGen[0] = best;

        List<MovementNode> sequence = best.Sequence;
        Debug.Log("Best Sequence) " + evaluateCreature(best).ToString() + toStringSequence(sequence));

        return nextGen;
    }

    private string toStringSequence(List<MovementNode> sequence)
    {
        string result = "";
        for(int i=0; i<sequence.Count; i++)
        {
            result += "\tNode" + i + ": " + toStringMovementNode(sequence[i]);
        }
        return result;
    }

    private string toStringMovementNode(MovementNode movement)
    {
        return "Delay = " + movement.Delay + " Force = " + movement.Force + " Segment = " + movement.Segment;
    }

    // find best creature
    private int findBest(List<Creature> creatures)
    {
        int index = 0;
        double eval = evaluateCreature(creatures[0]);
        for(int i=1; i<creatures.Count; i++)
        {
            double ev = evaluateCreature(creatures[i]);
            if(eval < ev)
            {
                index = i;
                eval = ev;
            }
        }

        return index;
    }

    List<Creature> crossover(List<Creature> creatures)
    {
        List<MovementNode> sequence1 = creatures[0].Sequence;
        List<MovementNode> sequence2 = creatures[1].Sequence;

        List<MovementNode> crossed1 = new List<MovementNode>();
        List<MovementNode> crossed2 = new List<MovementNode>();

        for (int i = 0; i < sequence1.Count; i++)
        {
            int randInt = rand.Next() % 2;
            if (randInt == 0)
            {
                crossed1.Add(sequence1[i]);
                crossed2.Add(sequence2[i]);
            }
            else
            {
                crossed1.Add(sequence2[i]);
                crossed2.Add(sequence1[i]);
            }
        }

        creatures[0].Sequence = crossed1;
        creatures[1].Sequence = crossed2;

        return creatures;
    }

    Creature mutate(Creature creature)
    {
        List<MovementNode> sequence = creature.Sequence;
        foreach (MovementNode mn in sequence)
        {
            mn.Direction = new Vector2(Random.Range(0f, 0.5f), Random.Range(0.5f, 2f));

            mn.Force = mn.Force * (1 + (float)rand.NextDouble() - 0.5F);
            mn.Force = mn.Force > Tournament.instance.maxForce ? Tournament.instance.maxForce : mn.Force;
            mn.Force = mn.Force < Tournament.instance.minForce ? Tournament.instance.minForce : mn.Force;

            mn.Delay = mn.Delay * (1 + (float)rand.NextDouble() - 0.5F);
            mn.Delay = mn.Delay > Tournament.instance.maxDelay ? Tournament.instance.maxDelay : mn.Delay;
            mn.Delay = mn.Delay < Tournament.instance.minDelay ? Tournament.instance.minDelay : mn.Delay;
        }

        for (int i = 0; i < sequence.Count; i++)
        {
            sequence[i].Segment = Random.Range(0, creature.getSegmentCount());
        }

        Creature mutation = creature;
        mutation.Sequence = sequence;

        return mutation;
    }


    private double evaluateCreature(Creature creature)
    {
        EvalFunction ef = new EvalFunction();
        return ef.evalFunction(creature);
    }

    // return random permutated list. e.g. [1 0 2], [0 2 1], [0, 1, 2]
    private List<int> shuffle(int num)
    {
        List<int> list = Enumerable.Range(0, num).ToList<int>();
        for (int i = 0; i < list.Count - 1; i++)
        {
            int index = rand.Next() % (num - 1 - i);
            int temp = list[index];
            list[index] = list[num - 1 - i];
            list[num - 1 - i] = temp;
        }

        return list;
    }

    // example 
    // input rultette is [0.1, 0.5, 0.7, 1]
    // generate randome real number in [0, 1]. assume randome number R = 0.3
    // return 1 because 0.1 < R < 0.5 and its index is 1
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

    // make normalized roulette. normalized roulette is comulated in range [0, 1]
    private List<double> getRoulette(List<Creature> creatures)
    {
        List<double> roulette = new List<double>();
        foreach (Creature creature in creatures)
        {
            roulette.Add(evaluateCreature(creature));
        }

        return comulateRoulette(normalizeRoulette(roulette));
    }

    // example
    // input [10 4 3 12 15 6]
    // return [0.2 0.08 0.06 0.24 0.3 0.12]
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
            if((int)sum == 0)
            {
                normalized.Add(1.0 / roulette.Count);
            }
            else
            {
                normalized.Add(f / sum);
            }
            
        }

        return normalized;
    }

    // example
    // input [0.2 0.08 0.06 0.24 0.3 0.12]
    // return [0.2 0.28 0.34 0.58 0.88 1]
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
