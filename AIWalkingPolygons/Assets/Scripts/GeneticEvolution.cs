using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneticEvolution
{
    public double mutatioinProb = 0.1;
    public double crossoverProb = 0.9;
    private System.Random rand = new System.Random();

    public List<Creature> CreateNextGen(List<Creature> creatures)
    {
        Debug.Log("Best Creature) " + toStringCreature(creatures[findBest(creatures)]));

        //List<Creature> nextGen = new List<Creature>();
        //for (int i = 0; i < creatures.Count; i++)
        //{
        //    nextGen.Add(creatures[0]);
        //}


        /////////////////////////////////////////////////////////////////////////
        int passNum = 6;
        List<Creature> nextGen = passParents(creatures, passNum);
        List<double> roulette = getRoulette(creatures);

        for (int i = 0; i < (creatures.Count - passNum) / 2; i++)
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

                child1 = mutate(child1);
                child2 = mutate(child2);
            }

            nextGen.Add(child1);
            nextGen.Add(child2);
        }

        return nextGen;
    }


    private List<Creature> passParents(List<Creature> creatures, int passNum)
    {
        List<Creature> tops = new List<Creature>();
        for (int i=0; i<passNum; i++)
        {
            creatures = swap(creatures, findBest(creatures.GetRange(i, creatures.Count-i)) + i, i);
            tops.Add(creatures[i]);
        }
        return tops;
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

    private List<Creature> swap(List<Creature> creatures, int index1, int index2)
    {
        Creature tempCreture = creatures[index1];
        creatures[index1] = creatures[index2];
        creatures[index2] = tempCreture;
        return creatures;
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

        if (rand.NextDouble() < mutatioinProb)
        {
            sequence = mutateDirection(sequence);
        }

        if (rand.NextDouble() < mutatioinProb)
        {
            sequence = mutateForce(sequence);
        }

        if (rand.NextDouble() < mutatioinProb)
        {
            sequence = mutateDelay(sequence);
        }

        if (rand.NextDouble() < mutatioinProb)
        {
            sequence = mutateSegment(sequence, creature.getSegmentCount());
        }

        Creature mutation = creature;
        mutation.Sequence = sequence;

        return mutation;
    }

    private List<MovementNode> mutateDirection(List<MovementNode> sequence)
    {
        foreach (MovementNode mn in sequence)
        {
            mn.Direction = new Vector2(Random.Range(0f, 0.5f), Random.Range(0.5f, 2f));
        }
        return sequence;
    }

    private List<MovementNode> mutateForce(List<MovementNode> sequence)
    {
        foreach (MovementNode mn in sequence)
        {
            mn.Force = mn.Force * (1 + (float)rand.NextDouble() - 0.5F);
            mn.Force = mn.Force > Tournament.instance.maxForce ? Tournament.instance.maxForce : mn.Force;
            mn.Force = mn.Force < Tournament.instance.minForce ? Tournament.instance.minForce : mn.Force;
        }
        return sequence;
    }

    private List<MovementNode> mutateDelay(List<MovementNode> sequence)
    {
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
        return sequence;
    }

    private List<MovementNode> mutateSegment(List<MovementNode> sequence, int numSegments)
    {
        foreach (MovementNode mn in sequence)
        {
            mn.Segment = Random.Range(0, numSegments);
        }
        return sequence;
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

    private string toStringCreature(Creature creature)
    {
        return "Evaluation: " + evaluateCreature(creature).ToString() + "\n" + toStringSequence(creature.Sequence);
    }

    private string toStringSequence(List<MovementNode> sequence)
    {
        string result = "";
        for (int i = 0; i < sequence.Count; i++)
        {
            result += "Node" + i + ": " + toStringMovementNode(sequence[i]);
            if (i != sequence.Count - 1)
                result += "\n";
        }
        return result;
    }

    private string toStringMovementNode(MovementNode movement)
    {
        return "Delay = " + movement.Delay + " Force = " + movement.Force + " Segment = " + movement.Segment;
    }
}
