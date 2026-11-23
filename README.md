# Galáxia Espiral — Unity 6

Abra `My project (1)` com Unity **6000.5.5f1** e abra a cena `Assets/Scenes/SampleScene.unity`. Aperte **Play**.

A cena aproveita o objeto **Galaxia Espiral** (Particle System original), com o componente **GalaxiaEspiral**. São 5.500 estrelas, 2.600 partículas de gás e 900 partículas no núcleo, divididas em três braços. As partículas têm movimentos orbitais distintos: as internas giram mais rápido. A câmera está de frente, com fundo escuro.

Para ajustar a aparência, selecione **Galaxia Espiral** e edite `Bracos`, `Raio`, `Voltas`, `Dispersao`, `Estrelas`, `Gas`, `Nucleo` e `Velocidade Angular`. Os valores são aplicados ao iniciar o Play.

**Importante:** a aparência final depende do material atribuído ao Particle System no Unity. Este projeto mantém o material original. Se as partículas aparecerem como quadrados, escolha no Renderer um material de partículas compatível com URP, com textura circular suave e transparência.

A execução no Editor não foi testada nesta entrega; confirme a cena em Play antes da apresentação.
