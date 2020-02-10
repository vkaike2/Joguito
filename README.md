# Versão Unity
- 2019.2.8f1


# INTERFACES
- IInteractable
    - int Order();

- IPickable : IInteractable
    - void PickUp();
- IPickableUI : IPickable

- IPlantable : IInteractable
    - void Interact();
- IPlantableUI : IPlantable 

- IDamageTaker : IInteractable
    - void TakeDamage(flaot damage);




