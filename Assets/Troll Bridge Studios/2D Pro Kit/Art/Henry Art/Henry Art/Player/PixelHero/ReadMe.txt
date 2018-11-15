PixelHero
art: benmhenry@gmail.com
code: davidahenry@gmail.com

Description:
Includes 16 by 16 animated (UI.Image & Sprite) pixel hero.<br>
Includes tintable layers for hair, accessories, shirt, pants and shoes.<br>
Included walk, two attack, and five idle animations in four cardinal directions.<br>
The animation controller will select a random idle animation each time.<br>
Included synced foot step sounds.<br>

Documentation:
Sprite:
0. Use prefabs or:
1. Create Animator and select Hero animation.
2. Add Audio source for each step and Audio script.
3. Create->2D->Sprite : Create 8 Sprite children. Body, Eyes, Hair, Pants, Shirt, Shoes, Quiver, and Cloak.
4. Set color of eyes.

UI.Image:
0. Use prefabs or:
1. Create Animator and select Hero animation.
2. Add Audio source for each step and Audio script.
3. Create->UI->Image : Create 8 Image children. Body, Eyes, Hair, Pants, Shirt, Shoes, Quiver, and Cloak.
4. Add Animator and select slime animation

Files:
PixelHero/Audio/PinDrop.wav : used for example scene intro
PixelHero/Audio/Step0.wav : left step
PixelHero/Audio/Step1.wav : right step
PixelHero/Prefab/HeroImage.prefab : example prefab
PixelHero/Prefab/HeroSprite.prefab : example prefab
PixelHero/Scene/HeroImage.unity : example scene
PixelHero/Scene/HeroSprite.unity : example scene
PixelHero/Scene/Intro.unity : example scene intro
PixelHero/Script/Audio.cs : plays foot steps
PixelHero/Script/Ease.cs : simple ease system used for bounce in example scene intro
PixelHero/Script/Intro.cs : used for example scene intro
PixelHero/Script/Hero.cs : example script switches between animation states
PixelHero/Script/StateRandom.cs : used in animation controllers to select random idle animation
PixelHero/Visual/Animation/Hero.controller : animation controllers
PixelHero/Visual/Animation/HeroTint.controller : animation controllers for tintable set
PixelHero/Visual/Font/SuperBlack.fontsettings (etc) : fonts used for example scene
PixelHero/Visual/Sprite/Hero/HeroBodyEast.png (etc) : layer & direction sprites on single sprite sheet
PixelHero/Visual/Sprite/Hero/Tint/HeroBodyEast.png (etc) : extra more tintable set
PixelHero/Visual/Sprite/Henry.png : used for example scene intro
PixelHero/ReadMe.txt : this
