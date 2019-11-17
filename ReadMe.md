Simple A* algorithm 
===============
Unity ����( Unity 2018.3.0f2 (64-bit) ) ���� ������ A* �˰��� ������Ʈ ���� �Դϴ�.

�� ����� ����� ���� "��������"�� ��ã�� �˰��� ���Ǿ����ϴ�.

�������� �ٿ�ε� ��ũ : https://play.google.com/store/apps/details?id=com.EXP.EXP_SooMolJiGoo&hl=ko

youtube ��ũ : https://youtu.be/eHvMHx2CLUQ

��ũ��Ʈ ����
--
	1. Node.cs : ������ Cell �Դϴ�.
	2. Grid.cs : ������ ũ�⸸ŭ ȭ���� Node �迭�� ä��ϴ�. 
	3. Pathfinding.cs : A* �˰��� �Դϴ�. Grid�� Ȱ���Ͽ� Target������ �ִܰŸ��� ã���ϴ�.


���� ���
--
	1. Player ���ӿ�����Ʈ�� �����մϴ�.
![player](./Images/player.png)

	2. Astar - Path Finding�� Seeker�� Player��, Grid�� Unwalkable ���̾ �����մϴ�.
 ![astar](./Images/astar.png)

	3. Obstacle�� layer�� Unwalkable�� ���� �� BoxCollider�� �߰��մϴ�.
 ![obstacle](./Images/obstacle.png)
 
 	4. Target�� Target.cs�� �����մϴ�. 
 ![target](./Images/target.png)
 
 	5. Target Game Object�� ��ġ�� ���� A*�� Target�� �����˴ϴ�. Scene View���� Target�� ��ġ�� ��ȭ���Ѽ� Ȯ���� �� �ֽ��ϴ�.
 ![play](./Images/play.png)
 