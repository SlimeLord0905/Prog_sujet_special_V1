# 420-1SS-SW: Développement: Sujets Spéciaux

## Projet : Gestionnaire de mots de passe

### Version #1

## Correction

1. Structure générale du projet (dépôt Git, solution, projets de bibliothèque de
   classes, application console, ...)
    - **note** : 3/3
2. Qualité du code (organisation du code, utilisation de méthodes appropriées,
   gestion des exceptions et des erreurs, ...)
    - **note** : 3/5
    - `Main` très long, tu devrais utiliser plusieurs méthodes pour alléger
      ton `Main`
    - clé de chiffrement *hard-codée* dans le code source, pas très sécuritaire
3. Tests unitaires (pour les classes/méthodes dans la bibliothèque de classes)
    - **note** : 0/5
    - tu m'avais demandé de donner un rappel sur les tests unitaires, j'ai donné
      plusieurs tests en exemple, et tu n'as aucun test unitaire dans ton projet
4. Génération d'un mot de passe
    - **note** : 2/2
    - sortie du JSON au complet après la création d'un mot de passe
5. Gestion du fichier de mots de passe (créer, ouvrir, enregistrer, lister)
    - **note** : 4/4
6. Chercher un mot de passe
    - **note** : 3/3
7. Sélectionner un mot de passe (déchiffrer, cacher, mettre à jour, effacer)
    - **note** : 6/8
    - après avoir mis à jour un mot de passe et avoir quitté le programme, il n'est pas possible de lire le fichier de mots de passe (peut-être parce que le mot de passe n'a pas été encrypté correctement ?) 
    - *Unhandled exception. System.Text.Json.JsonException: '0x04' is invalid after a single JSON value. Expected end of data. Path: $[3] | LineNumber: 17 | BytePositionInLine: 0.*

### Total : 23/30
