# Pegar um Item no Chão
- Clica no **ItemDropComponent**
- Chama o metodo **MovementMouseComponent.ObjectGoTo()** 
    - Player vai até o range do item
- Chama metodo **InteractComponent.SetInteraction()**
    - Seta que o player tem intenção de pegar o item
        - Caso player clique canceca a intenção
- Quando player esta no range do item, e possui a intensão de pegar
    - Chama **InventoryComponnent.AddItem()**
