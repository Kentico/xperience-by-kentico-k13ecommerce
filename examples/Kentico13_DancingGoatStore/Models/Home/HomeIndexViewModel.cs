﻿namespace DancingGoat.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<HomeSectionViewModel> HomeSections { get; set; }

        public ReferenceViewModel Reference { get; set; }
    }
}