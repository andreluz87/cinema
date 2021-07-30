using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinema.Models;
using System.Collections;

namespace Cinema.ViewModels
{
    public class SessionViewModel
    {
        private SelectList _AnimationTypeSelectList { get; set; }
        private SelectList _AudioSelectList { get; set; }

        public static readonly string[] AnimationTypes = { "2D", "3D" };
        public static readonly string[] Audios = { "Legendado", "Dublado" };

        public class RoomsListViewModel
        {
            public IEnumerable<Room> Rooms { get; set; }
        }

        public SelectList AnimationTypeSelectList
        {
            get
            {
                if (_AnimationTypeSelectList != null)
                    return _AnimationTypeSelectList;

                return new SelectList(
                    new List<SelectListItem>
                        {
                            new SelectListItem { Text = "2D", Value = "0"},
                            new SelectListItem { Text = "3D", Value = "1" },
                        }, "Value", "Text", 0
                );
            }

            set { _AnimationTypeSelectList = value; }
        }

        public SelectList AudioSelectList
        {
            get
            {
                if (_AudioSelectList != null)
                    return _AudioSelectList;

                return new SelectList(
                    new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Legendado", Value = "0"},
                            new SelectListItem { Text = "Dublado", Value = "1" }
                        }, "Value", "Text", 0
                );
            }

            set { _AudioSelectList = value; }
        }       
    }
}
