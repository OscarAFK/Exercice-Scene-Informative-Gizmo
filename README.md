# Scene Informative Gizmo
The objective of this exercice was to add a Scene Informative Gizmo tool, that would allow to display information about a level using gizmos, and to quickly edit these informations.

![Exemple](https://github.com/OscarAFK/Exercice-Scene-Informative-Gizmo/blob/main/presentation.jpg?raw=true)

The informations were stored in a ScriptableObject, and were a list of String labels associated with a Vector3 position.

## Postmortem

The tricky part of this exercice was to avoid using gameobjects to display gizmos, something that I never did before. But after some trial and error, and extensive documentation readings, I figured out how to achieve all the requirements.
Overall it was really interesting, and I learned much.
