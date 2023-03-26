using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;
namespace Data
{
    internal sealed class SectionLogic : ISectionLogic
    {
        public readonly IStorage Storage;
        private readonly SectionMarkupHandler SectionMarkupHandler;
        public SectionLogic(IStorage storage,
        SectionMarkupHandler sectionMarkupHandler)
        {
            Storage = storage;
            SectionMarkupHandler = sectionMarkupHandler;
        }
        public void AddSection(int Num)
        {
            int number = Num;
            int id = number + 1;
            int count = Storage.Slow.CountThreadsById(id);

            if (count == 0)
                count++;
            int pagesCount = count / Constants.threadsOnPage;

            if (count - pagesCount * Constants.threadsOnPage > 0)
                pagesCount++;
            Storage.Fast.SetSectionPagesArrayLocked
                            (number, new string[pagesCount]);
            Storage.Fast.
                SetSectionPagesPageDepthLocked(number, pagesCount);

            for (int i = 0; i < pagesCount; i++)
                Storage.Fast.SetSectionPagesPageLocked(number, i, Constants.buttonTxt);

            ProcessSectionReader(Storage.Slow.GetThreadIdNamesById(id),
                number, pagesCount);
        }
        public void RemoveBrOfIncompletePages(int number)
        {
            string temp = Storage.Fast.GetSectionPagesPageLocked(number,
                   Storage.Fast.GetSectionPagesArrayLocked(number).Length - 1);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Storage.Fast.SetSectionPagesPageLocked(number,
                Storage.Fast.GetSectionPagesArrayLocked(number).Length - 1, temp);
        }

        public void ProcessSectionReader
            (IEnumerable<IdName> idNames, int number, int pagesCount)
        {
            if (idNames.Any())
            {
                string endpointHidden = Constants.SE;
                int pageNumber = 0;
                int i = 0;

                foreach (var idName in idNames)
                {
                    if (i == 0)
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, SectionMarkupHandler.GetNav());
                    int id_ = idName.Id;

                    endpointHidden = SectionMarkupHandler
                        .GetEndpointHidden(Storage.Slow.GetSectionNumById(id_));

                    Storage.Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, SectionMarkupHandler
                            .GetThreadLinkText(id_, idName.Name));
                    i++;

                    if (i == Constants.threadsOnPage)
                    {
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            SectionMarkupHandler.SetNavigationWithEndpoint(pageNumber,
                                pagesCount, number, endpointHidden));

                        i = 0;
                        pageNumber++;
                    }
                    else
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompletePages(number);

                if ((i < Constants.threadsOnPage) && (i > 0))
                {
                    if (pageNumber > 0)
                    {
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            SectionMarkupHandler
                                .SetNavigation(pageNumber, pagesCount, number));
                    }
                    Storage.Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, SectionMarkupHandler.GetNavWithEndpoint(endpointHidden));
                }
            }
        }

        public string GetSectionPage(int? Id, int? page)
        {
            if (Id == null || page == null)
                return Constants.SE;
            else
            {
                if (Id > 0 && Id <= Storage.Fast.GetSectionPagesLengthLocked())
                {
                    int index = (int)Id - 1;

                    if (page > 0
                        && page <= Storage.Fast.GetSectionPagesPageDepthLocked(index))
                        return Storage.Fast.GetSectionPagesPageLocked
                            (index, (int)page - 1);
                    else return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadSectionPages()
        {
            Storage.Fast.SetSectionPagesLengthLocked(Constants.len);
            Storage.Fast.InitializeSectionPagesPageDepthLocked(
                new int[Storage.Fast.GetSectionPagesLengthLocked()]);
            Storage.Fast.InitializeSectionPagesLocked
                (new string[Storage.Fast.GetSectionPagesLengthLocked()][]);

            for (int i = 0;
                i < Storage.Fast.GetSectionPagesLengthLocked(); i++)
            {
                AddSection(i);
            }
        }
    }
}
