using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class ZombieBotControllerTests
{
    [UnityTest]
    public IEnumerator ZombieBotController_StartsAiWalking()
    {
        GameObject zombieGameObject = new GameObject();
        ZombieBotController zombieBotController = zombieGameObject.AddComponent<ZombieBotController>();
        // Create an Animator component and attach it to the GameObject
        Animator animator = zombieGameObject.AddComponent<Animator>();
        zombieBotController.GetComponent<Animator>();

        // Set up the required animator parameters (you may need to adjust this based on your actual implementation)
        zombieBotController.isWalkingHash = Animator.StringToHash("isWalking");
        zombieBotController.isRunningHash = Animator.StringToHash("isRunning");
        zombieBotController.isAttackingHash = Animator.StringToHash("isAttacking");

        yield return null; // Wait for one frame

        Assert.AreEqual(ZombieBotController.ZombieState.AiWalking, zombieBotController.currentState);
    }

    [UnityTest]
    public IEnumerator ZombieBotController_StartsControlling()
    {
        GameObject zombieGameObject = new GameObject();
        ZombieBotController zombieBotController = zombieGameObject.AddComponent<ZombieBotController>();
        // Create components and attach it to the GameObject
        Animator animator = zombieGameObject.AddComponent<Animator>();
        CharacterController  characterController = zombieGameObject.AddComponent<CharacterController>();
        NavMeshAgent navMeshAgent = zombieGameObject.AddComponent<NavMeshAgent>();
        zombieBotController.animator = animator;
        zombieBotController.characterController = characterController;
        zombieBotController.navMeshAgent = navMeshAgent;

        // Set up the required animator parameters (you may need to adjust this based on your actual implementation)
        zombieBotController.isWalkingHash = Animator.StringToHash("isWalking");
        zombieBotController.isRunningHash = Animator.StringToHash("isRunning");
        zombieBotController.isAttackingHash = Animator.StringToHash("isAttacking");

        zombieBotController.StartControlling();

        yield return null; // Wait for one frame

        Assert.AreEqual(ZombieBotController.ZombieState.Controlled, zombieBotController.currentState);
    }

    [UnityTest]
    public IEnumerator ZombieBotController_TakesDamage_Dies()
    {
        GameObject zombieGameObject = new GameObject();
        ZombieBotController zombieBotController = zombieGameObject.AddComponent<ZombieBotController>();
        // Create components and attach it to the GameObject
        Animator animator = zombieGameObject.AddComponent<Animator>();
        CharacterController characterController = zombieGameObject.AddComponent<CharacterController>();
        NavMeshAgent navMeshAgent = zombieGameObject.AddComponent<NavMeshAgent>();
        AudioSource audioSource = zombieGameObject.AddComponent<AudioSource>();
        zombieBotController.animator = animator;
        zombieBotController.characterController = characterController;
        zombieBotController.navMeshAgent = navMeshAgent;
        zombieBotController.hurtAudioSource = audioSource;
        zombieBotController.ragdollRigidbodies = new Rigidbody[1];
        Rigidbody body = zombieGameObject.AddComponent<Rigidbody>();
        zombieBotController.ragdollRigidbodies[0] = body;
        zombieBotController.ragdollRigidbodies[0].isKinematic = false;

        // Set up the required animator parameters (you may need to adjust this based on your actual implementation)
        zombieBotController.isWalkingHash = Animator.StringToHash("isWalking");
        zombieBotController.isRunningHash = Animator.StringToHash("isRunning");
        zombieBotController.isAttackingHash = Animator.StringToHash("isAttacking");

        zombieBotController.TakeDamage((int)zombieBotController.maxHealth);

        yield return null; // Wait for one frame

        Assert.IsTrue(zombieBotController.isDead);
        Assert.AreEqual(ZombieBotController.ZombieState.Ragdoll, zombieBotController.currentState);
    }


    [UnityTest]
    public IEnumerator ZombieBotController_CanRunWhileControlled()
    {
        GameObject zombieGameObject = new GameObject();
        ZombieBotController zombieBotController = zombieGameObject.AddComponent<ZombieBotController>();
        // Create components and attach it to the GameObject
        Animator animator = zombieGameObject.AddComponent<Animator>();
        CharacterController characterController = zombieGameObject.AddComponent<CharacterController>();
        NavMeshAgent navMeshAgent = zombieGameObject.AddComponent<NavMeshAgent>();
        zombieBotController.animator = animator;
        zombieBotController.characterController = characterController;
        zombieBotController.navMeshAgent = navMeshAgent;

        // Set up the required animator parameters (you may need to adjust this based on your actual implementation)
        zombieBotController.isWalkingHash = Animator.StringToHash("isWalking");
        zombieBotController.isRunningHash = Animator.StringToHash("isRunning");
        zombieBotController.isAttackingHash = Animator.StringToHash("isAttacking");

        zombieBotController.StartControlling();
        zombieBotController.OnControlledRun();

        yield return null; // Wait for one frame

        Assert.IsTrue(zombieBotController.isControlledRunning);
    }
}
