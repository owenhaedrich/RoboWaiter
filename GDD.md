# RoboWaiter Simcade Prototype

## Overview

Play as a one-wheeled robot who makes their living as a waiter. Deliver plates to tables as quickly and accurately as possible without losing your balance. The robot's single wheel makes every delivery feel precarious, turning simple tasks, like navigating around a chair or slowing down at a corner, into challenges. The player must constantly modulate speed, lean, and turn the proper direction to stay balanced and upright.

## Objective Statement

Does balancing a unicycle-like player create mixed feelings of frustration and satisfaction?

We believe that the balancing mechanic can be a strong central gameplay mechanic, however, it risks being either too hard and offputting, or too easy and unnecessary. Our goal is to find a balance between these extremes and create a healthy challenge. We want to determine whether balance based movement mechanics provides a satisfying difficulty curve where players jump between brief moments of frustration and clear moments of success, creating an emotional arc that feels both challenging and rewarding.

## Design Rationale

The robot waiter scenario satisfies the needs of our objective statement with a reasonable scope. The enclosed restaurant environment limits our total asset requirements. We can leverage Unity’s built-in wheel collider, rigidbody, and fixed joint physics components to completely model our scenario.

Since the balancing system is fully physics driven, players understand why they fail. When they tip, wobble, or overcorrect, the cause is visually and mechanically clear. This approach also lets us experiment with difficulty by adjusting physical properties (mass, drag, torque, friction) instead of building custom complex logic. The restaurant environment also offers built-in opportunities that encourage replayability. This can be achieved by changing table numbers each level or upon restart, moving or randomizing obstacles, and varying orders (weights) each run. Additionally, camera perspective options allow players to choose views that make the experience easier or more challenging.

The camera design also plays an important role in supporting the design mechanic. We used two cameras that allowed the player to choose between two perspectives by hitting the **TAB** button. The first perspective is a fixed and top-down camera, giving a greater sense of spatial awareness to the player and a more steady view. The second perspective is a third-person camera positioned closer behind the robot's head, showing its shoulders and raised arms that hold the trays. This perspective makes navigation more difficult as the camera sways and follows the players movements, making it easier to wobble or tip over. Allowing the players to switch between these perspectives adds a layer of strategy, accessibility, and personal preference, while still focusing on the core mechanics of balancing.

## Unicycle Mechanics

Unicycle wheels turn when leaned. This is due to **camber thrust**. When the wheel is leaned towards one side, the contact area with the ground is made of a cone-like shape (see the diagram below). The large radius travels a longer distance per rotation than the small radius, therefore, the wheel must rotate towards the small radius. We simulate this effect by applying a torque that scales with the speed and angle of the wheel.

![Camber Thrust Diagram](https://i.imgur.com/LX82R6J.png)

***Wheel Physics Investigation Diagram: Camber Thrust***

## Metric Research and References

The RoboWaiter carries trays with real-world weights and mass, the game can simulate physics-based balancing. This creates a gameplay loop where managing balance is directly tied to the weight and player’s ability to stay balanced while moving.

The player’s balance is influenced not only by the robot’s own mass but also by the objects it carries. Each plate adds weight and shifts the center of mass, dynamically affecting the robots stability. For example, a single burger may add only a small amount of mass near the center of the tray, while heavier items like a whole chicken or stacked plates create greater torque and a higher center of mass. These shifts make leaning, turning, and stopping more challenging, requiring the player to constantly adjust their tilt and wheel rotation.

| Food item | Approx real world weight | Plate name in game | Source and Notes |
| :--- | :--- | :--- | :--- |
| Plate of fries | 0.200kg | | [https://foodstruct.com/food/french-fries](https://foodstruct.com/food/french-fries) and doubled the amount |
| Bun | 0.050kg | | [https://www.walmart.ca/en/ip/dempsters-original-hamburger-buns/6000187459426](https://www.walmart.ca/en/ip/dempsters-original-hamburger-buns/6000187459426) |
| Burger Patty | 0.113kg | | [https://www.walmart.ca/en/ip/Great-Value-Frozen-Beef-Value-Pack-Burgers/6000191272333](https://www.walmart.ca/en/ip/Great-Value-Frozen-Beef-Value-Pack-Burgers/6000191272333) |
| Burger on plate | 0.874kg | | Added three weights together |
| Burger and Fries on Plate | 1.074kg | Plate02 | Added four weights together |
| Water Glass | 0.566kg | | Taija weighed a water glass at home |
| Full Water Glass | 0.941kg | | Taija weighed a full water glass at home |
| Soup | 0.284kg | | [https://www.walmart.ca/en/ip/Campbell-s-Condensed-Chicken-Noodle-Soup-Shelf-Stable/10142058](https://www.walmart.ca/en/ip/Campbell-s-Condensed-Chicken-Noodle-Soup-Shelf-Stable/10142058) |
| Soup in Bowl | 0.627kg | | Added two weights together |
| Soup bowl, bun, filled watercup | 1.618kg | Plate01 | Added three weights together |
| Pie slice | 0.106kg | | [https://www.walmart.ca/en/ip/your-fresh-market-lime-meringue-pie/6S6JG77C3GMA](https://www.walmart.ca/en/ip/your-fresh-market-lime-meringue-pie/6S6JG77C3GMA) |
| Pie slice on plate | 0.817kg | Plate03 | Added two weights together |
| Plate | 0.711kg | | Taija weighed plates at home |
| Bowl | 0.343kg | | Taija weighed bowls at home |
| Coffee mug | 0.341kg | | Taija weighed a coffee mug at home |
| Coffee mug filled | 0.662kg | | |

![Scale Results](https://i.imgur.com/3yuUSok.png)
***Real World Scale Results***

## Playtest Feedback and Surrounding Discussion

For our playtest, we had a tight timeline so we asked three simple and yet open-ended questions: “How does it feel to move as the RoboWaiter?”, “What are your biggest concerns with the prototype?”, and “What are your ideas for elevating the prototype?”

Players described *RoboWaiter* as a unique and entertaining concept, with several expressing that the idea of a one-wheeled robot waiter was “pretty cool” and “super fun with a lot of potential.” Many testers enjoyed the chaotic nature of the physics, with one saying that “trying to stay upright while careening around was fun,” while another compared the experience to “a foddian rage game like QWOP.” Even when difficult, players felt that the jankiness contributed to the game’s personality, with one tester saying that it created “really funny moments out of just the camera alone.” Many players also expressed confusion about what to do at the start of the game. A simple start screen, control/instructions screen, or UI would solve this.

The most apparent criticism across all the feedback was the camera. Players described it as “VERY crazy,” “nauseating,” “too much,” and “hard to see what you’re doing at times.” Although a few testers felt the chaotic camera “almost adds to the experience,” the overall feedback was that a more stable or partially-following camera would improve the player experience without totally losing all the chaos the original camera brought.

Movement and balancing were described as fun but sometimes “too” janky. Testers expressed that stopping would sometimes cause the robot to fling back and forth “like one of those spring doorstops.” Turning created really wide leans that were hard to correct, and once the robot fell over, some players stated they couldn't get back up. Some players enjoyed the difficulty curve these issues created, but others felt it could be more forgiving and better.

Interaction with the plates also raised some issues. While the grabbing plates function worked well, placing them back on counters or surfaces was “pretty hard to do.” However, this issue is not a real concern, as it serves only as a placeholder for the actual function, which is the player gets within a certain distance from the table and it will automatically be placed down onto it. One player did mention that “the plates spawn a bit too quickly to deliver them fast,” which added unnecessary pressure where we didn’t want and was tweaked for the final prototype.

Even with these issues, players still expressed the game’s potential. A few offered ideas for the game such as, imagining it as “cafe theme” or delivering drinks “in a certain amount of time without spilling stuff.” The concept overall is strong, the physics were entertaining, and with improved camera control, UI, and fine-tuned interactions, the game would tick all the boxes of the testers wants and wishes.

## Foddian Game Appeal

*RoboWaiter* targets the niche of Foddian games. Bennett Foddy popularized this niche with his highly challenging games, such as *QWOP* and *Getting Over It*, where simply moving the player character requires precise inputs. We aim to fill this niche by making maneuvering our RoboWaiter a challenge by requiring the players to control their speed and lean forces precisely to avoid falling over.

## Visual Considerations

The overall aesthetic of *RoboWaiter* is **Retro-Futurism**. The idea of the 50’s vibe with futuristic robots was inspired by games like *Fallout 4*. The game itself is quirky and kitschy and the 50's falls right into that category. The diner’s patrons include a mix of humans, robots, droids, and other humanoids. Classic 50's music would play from the jukebox to further immerse the player in the environment.

Visually, the diner would feature a bright, iconic palette of red, white, and black. The WaiterBot itself would be silver and blue, with a metal texture. Its rounded, soft edges help give it a cute and friendly personality.

Key considerations included maintaining proper scale and proportions throughout the environment and ensuring the space wasn’t too visually cluttered.

![RoboWaiter Model](https://i.imgur.com/Kr8iNYD.png)
***RoboWaiter Player: Modeled in 3DSMax***

## Future Developments

* Picking up finished plates bringing them to dishwasher/sink
* Changing of table numbers
* If the player is fast and efficient, customers leave tips that can be used for upgrades (faster wheel power, time slow, etc).
* Adding a two player where one plays as the WaitorBot and one plays as the cook and has to plate orders correctly following off an order ticket
* Adding obstacles:
    * Customers walking
    * Coffee/water spilt on the ground (can be remedied by putting out the wet floor sign)
    * Chairs

## Citations

1.  FoodStruct. (2025). *French fries nutrition: calories, carbs, GI, protein, fiber, fats*. FoodStruct. [https://foodstruct.com/food/french-fries](https://foodstruct.com/food/french-fries)
2.  Katydid. (2025). *For PashaPasha’s Diner* [3D model]. Sketchfab. [https://sketchfab.com/3d-models/for-pashapashas-diner-f77bd8aae0ab4b77a897a8c7ede5ecaf](https://sketchfab.com/3d-models/for-pashapashas-diner-f77bd8aae0ab4b77a897a8c7ede5ecaf)
3.  Jackson, J. (Director). (2019, August 2). *Camber Thrust—Why a Motorcycle turns* [Video recording]. [https://www.youtube.com/watch?v=Qf7yKTX__TM](https://www.youtube.com/watch?v=Qf7yKTX__TM)
4.  JungleJim. (2023). *Ramen bowl* [3D model]. Sketchfab. [https://sketchfab.com/3d-models/ramen-bowl-374aba41259447e792b13ca99747a281](https://sketchfab.com/3d-models/ramen-bowl-374aba41259447e792b13ca99747a281)
5.  AlexixoAlonso (2019). *Tall drinking glass* [3D model]. Sketchfab. [https://sketchfab.com/3d-models/tall-drinking-glass-6e1168814f8d4c44b4a833f41093905a](https://sketchfab.com/3d-models/tall-drinking-glass-6e1168814f8d4c44b4a833f41093905a)
6.  Realbuilderai. (2023). *Cheese cake* [3D model]. Sketchfab. [https://sketchfab.com/3d-models/cheese-cake-7290e12a31d04cb3892049f379c4bafb](https://sketchfab.com/3d-models/cheese-cake-7290e12a31d04cb3892049f379c4bafb)
7.  Ustal, S. (2020). *Tasty burger with fries* [3D model]. Sketchfab. [https://sketchfab.com/3d-models/tasty-burger-with-fries-c624b290938c411880ec5254091ab572](https://sketchfab.com/3d-models/tasty-burger-with-fries-c624b290938c411880ec5254091ab572)
8.  Unicycle.com (UK) (Director). (2024, May 8). *How to turn on a unicycle* [Video recording]. [https://www.youtube.com/watch?v=0eM7QvwN5F0](https://www.youtube.com/watch?v=0eM7QvwN5F0)
9.  Walmart Canada. (2025). *Campbell’s Condensed Chicken Noodle Soup – Shelf Stable*. Walmart. [https://www.walmart.ca/en/ip/Campbell-s-Condensed-Chicken-Noodle-Soup-Shelf-Stable/10142058](https://www.walmart.ca/en/ip/Campbell-s-Condensed-Chicken-Noodle-Soup-Shelf-Stable/10142058)
10. Walmart Canada. (2025). *Dempster’s Original Hamburger Buns*. Walmart. [https://www.walmart.ca/en/ip/dempsters-original-hamburger-buns/6000187459426](https://www.walmart.ca/en/ip/dempsters-original-hamburger-buns/6000187459426)
11. Walmart Canada. (2025). *Great Value Frozen Beef Value Pack Burgers*. Walmart. [https://www.walmart.ca/en/ip/Great-Value-Frozen-Beef-Value-Pack-Burgers/6000191272333](https://www.walmart.ca/en/ip/Great-Value-Frozen-Beef-Value-Pack-Burgers/6000191272333)
12. Walmart Canada. (n.d.). *Your Fresh Market Lime Meringue Pie*. Walmart. [https://www.walmart.ca/en/ip/your-fresh-market-lime-meringue-pie/6S6JG77C3GMA](https://www.walmart.ca/en/ip/your-fresh-market-lime-meringue-pie/6S6JG77C3GMA)
13. YES Hire. (2025). *8oz water glass*. YES Hire. [https://yeshire.uk/8oz-water-glass/](https://yeshire.uk/8oz-water-glass/)
