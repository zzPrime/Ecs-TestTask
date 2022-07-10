Количество часов ~ 14

Ecs системы которые отвечают за расчёт логики, не зависят от Unity и могут быть запущены на сервере (при условии наличия на нём Leopotam.EcsLite и Leopotam.EcsLite.Di):

SetPlayerTargetSys
SetPlayerTranslationMethodSys
SetPlayerAnimationStateSys
CheckTriggersStateSys
SetLinkedObserverStateSys
SetDoorsTargetSys
SetDoorsMovementMethodSys
MoveByLerpLogicSys
RotateByConstantLogicSys
CheckMovementCompleteSys
CheckRotationCompleteSys

Системы для работы которых нужна развёрнутая Unity:

UnitySettingsBindingSys
UnityLevelInitializeSys
UnityMonobehViewSpawnSys
UnityBindSpawnedMonobehsSys
UnityMouseInputSys
UnityMonobehViewUpdateSys
UnityAnimationMonobehViewUpdateSys

Ссылка на видео:
https://drive.google.com/file/d/1_pkHBPUygltDreSY68K8zM9g98QZO3nR/view?usp=sharing

Примечания

В системах намеренно в основном использован интерфейс IEcsRunSystem. В моём понимании с подобным подходом гораздо удобнее расширять логику, в случае если в какой то момент игрового флоу требуется её перезапуск. Это будет проще реализовать чем перезапускать полностью все системы, с той только целью чтобы заново вызвать Init.

Для разрешения зависимостей из юнтити в Startup использован Zenject, внутрь систем инжекция производится через using Leopotam.EcsLite.Di. Использование встроеного Di гораздо удобнее учитывая что это отрывает удобный и лаконичный функционал EcsFilterInject и EcsPoolInject. Это гораздо проще и быстрее чем пытаться пробросить их через Zenject. Впрочем, наверное это уже вкусовщина, и наверняка найдутся те кто бы со мной поспорил)