using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
     // Scenario: If an item should be first but was added last, it should be dequeued first.
    // Expected Result: Dequeue returns the item with the highest priority regardless of insertion order.
    // Defect(s) Found: Dequeue must scan full list, remove the chosen item, and handle ties stably.
    public void TestPriorityQueue_1()
    {
var priorityQueue = new PriorityQueue();

        priorityQueue.Enqueue("Mulubah", 1);
        priorityQueue.Enqueue("Kemmeh", 5);
        priorityQueue.Enqueue("Mulubahzumu", 7);
        priorityQueue.Enqueue("Sipor", 10);

        // Highest-priority item should be returned first
        var first = priorityQueue.Dequeue();
        Assert.AreEqual("Sipor", first, "Highest priority item should be dequeued first.");

        // Next should be medium then low
        var second = priorityQueue.Dequeue();
        Assert.AreEqual("Mulubahzumu", second, "Second highest priority item expected.");

        var third = priorityQueue.Dequeue();
        Assert.AreEqual("Kemmeh", third, "Mid priority item expected after second highest priority.");

        var fourth = priorityQueue.Dequeue();
        Assert.AreEqual("Mulubah", fourth, "Lowest priority item expected last");
    }

    [TestMethod]
     // Scenario: Items with the same priority are dequeued in the order they were enqueued (stable behavior).
    // Expected Result: When priorities tie, the earlier-enqueued item is returned first.
    // Defect(s) Found: Using '>=' in comparison or updating index incorrectly breaks stability.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();

        priorityQueue.Enqueue("Mulubah", 1);
        priorityQueue.Enqueue("Kemmeh", 4);
        priorityQueue.Enqueue("Mulubahzumu", 10);
        priorityQueue.Enqueue("Sipor", 10);

        // Highest-priority item should be returned first
        var first = priorityQueue.Dequeue();

        // Next should be medium then low
        var second = priorityQueue.Dequeue();

        var third = priorityQueue.Dequeue();

        var fourth = priorityQueue.Dequeue();
        
        Assert.AreEqual("Mulubahzumu", first, "When priorities tie, the first enqueue should be dequeue");

        
        Assert.AreEqual("Sipor", second, "When priorities tie, the first enqueue should be dequeue");

        
        Assert.AreEqual("Kemmeh", third, "Second tied item should be returned after the first.");

        
        Assert.AreEqual("Mulubah", fourth, "Lowest priority item expected last");
    }

    // Add more test cases as needed below.
}