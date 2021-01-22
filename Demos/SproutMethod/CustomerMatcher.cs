using System;
using System.Text.RegularExpressions;

namespace SproutMethod
{
    public class CustomerMatcher
    {
        private readonly Customer Source;

        public CustomerMatcher(Customer source)
        {
            this.Source = source;
        }


        public bool Match(Customer target, MatchingCriteria criteria)
        {
            bool isMatch = false;

            var sourceName= this.Source.Name.ToUpperInvariant().Trim();
            var targetName = target.Name.ToUpperInvariant().Trim();

            if (criteria == MatchingCriteria.NameWholeWords)
            {
                sourceName = sourceName.Replace(".", " ");
                targetName = targetName.Replace(".", " ");
            }
            else
            {
                sourceName = sourceName.Replace(".", string.Empty);
                targetName = targetName.Replace(".", string.Empty);
            }

            switch (criteria)
            {
                case MatchingCriteria.Id:
                    {
                        return Source.Id == target.Id;
                    }
                case MatchingCriteria.NamePartial:
                    {
                        if (sourceName.Length > 0 && targetName.Length > 0)
                        {
                            return sourceName.Contains(targetName) || targetName.Contains(sourceName);
                        }

                        break;
                    }
                case MatchingCriteria.NameFirst4OrMore:
                    {
                        if (sourceName.Length > 0 && targetName.Length > 0)
                        {
                            var sourceFirstWord = GetFirstWord(sourceName);
                            sourceFirstWord = KeepLettersAndNumbers(sourceFirstWord);
                            if (sourceFirstWord.Length >= 4)
                            {
                                isMatch = $" {targetName} ".Contains($" {sourceFirstWord} ");
                                if (isMatch)
                                    return true;
                            }
                            if (!isMatch)
                            {
                                var targetFirstWord = GetFirstWord(targetName);
                                targetFirstWord = KeepLettersAndNumbers(targetFirstWord);
                                if (targetFirstWord.Length >= 4)
                                {
                                    isMatch = $" {sourceName} ".Contains($" {targetFirstWord} ");
                                    if (isMatch)
                                        return true;
                                }
                            }
                        }

                        break;
                    }

                case MatchingCriteria.NameFirst3:
                    {
                        if (sourceName.Length > 0 && targetName.Length > 0)
                        {
                            var sourceFirstWord = GetFirstWord(sourceName);
                            sourceFirstWord = KeepLettersAndNumbers(sourceFirstWord);
                            if (sourceFirstWord.Length == 3)
                            {
                                isMatch = $" {targetName} ".Contains($" {sourceFirstWord} ");
                                if (isMatch)
                                    return true;
                            }
                            if (!isMatch)
                            {
                                var targetFirstWord = GetFirstWord(targetName);
                                targetFirstWord = KeepLettersAndNumbers(targetFirstWord);
                                if (targetFirstWord.Length == 3)
                                {
                                    isMatch = $" {sourceName} ".Contains($" {targetFirstWord} ");
                                    if (isMatch)
                                        return true;
                                }
                            }
                        }

                        break;
                    }

                case MatchingCriteria.NameExact:
                    {
                        return sourceName == targetName;
                    }

                case MatchingCriteria.NameWholeWords:
                    {
                        foreach (char delimiter in new[] { ' ', '|' })
                        {
                            foreach (var splittedSourceName in sourceName.Split(delimiter))
                            {
                                if (splittedSourceName.Length >= 4)
                                {
                                    foreach (var splittedTargetName in targetName.Split(delimiter))
                                    {
                                        if (splittedTargetName.Length >= 4)
                                        {
                                            isMatch = (splittedSourceName == splittedTargetName);
                                            if (isMatch)
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }

                case MatchingCriteria.Phone:
                    {
                        if (Source.Phone.Length > 0 && target.Phone.Length > 0)
                        {
                            var cleanedSourcePhone = Regex.Replace(Source.Phone, @"\D*", string.Empty);
                            var cleanedTargetPhone = Regex.Replace(target.Phone, @"\D*", string.Empty);
                            isMatch = (cleanedSourcePhone == cleanedTargetPhone);
                            if (isMatch)
                                return true;
                            if (!isMatch)
                            {
                                cleanedSourcePhone = cleanedSourcePhone.Length > 9 ? cleanedSourcePhone.Substring(cleanedSourcePhone.Length - 9, 9) : "";
                                cleanedTargetPhone = cleanedTargetPhone.Length > 9 ? cleanedTargetPhone.Substring(cleanedTargetPhone.Length - 9, 9) : "";
                                if (cleanedSourcePhone.Length > 0 && cleanedTargetPhone.Length > 0)
                                {
                                    isMatch = (cleanedSourcePhone == cleanedTargetPhone);
                                    if (isMatch)
                                        return true;
                                }
                            }
                        }

                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return isMatch;
        }

        private string KeepLettersAndNumbers(string word)
        {
            return Regex.Replace(word, @"\W*", string.Empty).Replace("_", string.Empty);
        }

        private string GetFirstWord(string name)
        {
            if (name.IndexOf(" ") > 0)
            {
                return name.Split(" ")[0];
            }
            return name;
        }
    }
}
