Simple A* algorithm 
===============
Unity ����( Unity 2018.3.0f2 (64-bit) ) ���� ������ A* �˰��� ������Ʈ ���� �Դϴ�.

�� ����� ����� ���� "��������"�� ��ã�� �˰��� ���Ǿ����ϴ�.

PathFinding.cs : https://github.com/SeongjinBak/simple-Astar-algorithm/blob/master/simple_astar_algorithm/Assets/PathFinding.cs

�������� �ٿ�ε� ��ũ : https://play.google.com/store/apps/details?id=com.EXP.EXP_SooMolJiGoo&hl=ko

youtube ��ũ : https://youtu.be/eHvMHx2CLUQ

���� ������ : https://www.youtube.com/watch?v=-L-WgKMFuhE

��ũ��Ʈ ����
--
	1. Node.cs : ������ Cell �Դϴ�.
	2. Grid.cs : ������ ũ�⸸ŭ ȭ���� Node �迭�� ä��ϴ�. 
	3. Pathfinding.cs : A* �˰��� �Դϴ�. Grid�� Ȱ���Ͽ� Target������ �ִܰŸ��� ã���ϴ�.


���� ���
--
	1. Player ���ӿ�����Ʈ�� �����մϴ�.
![player](./Images/player.PNG)

	2. Astar - Path Finding�� Seeker�� Player��, Grid�� Unwalkable ���̾ �����մϴ�.
 ![astar](./Images/astar.PNG)

	3. Obstacle�� layer�� Unwalkable�� ���� �� BoxCollider�� �߰��մϴ�.
 ![obstacle](./Images/obstacle.PNG)
 
 	4. Target�� Target.cs�� �����մϴ�. 
 ![target](./Images/target.PNG)
 
 	5. Target Game Object�� ��ġ�� ���� A*�� Target�� �����˴ϴ�. Scene View���� Target�� ��ġ�� ��ȭ���Ѽ� Ȯ���� �� �ֽ��ϴ�.
 ![play](./Images/play.PNG)
 