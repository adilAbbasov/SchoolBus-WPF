using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBusWPF.ViewModels
{
    public class GroupViewModel : ViewModelBase
	{
		private Group _groupData;
		public Group GroupData
		{
			get { return _groupData; }
			set
			{
				_groupData = value;
				OnPropertyChanged(nameof(GroupData));
			}
		}

		private ObservableCollection<Group> _groups = [];
		public ObservableCollection<Group> Groups
		{
			get { return _groups; }
			set
			{
				_groups = value;
				OnPropertyChanged(nameof(Groups));
			}
		}

		private string? _title;
		[Required]
		public string? Title
		{
			get { return _title; }
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
				ValidateProperty(nameof(Title), value, "Title is required.");
			}
		}

        public GroupViewModel()
		{
			Groups = new ObservableCollection<Group>([.. _dbContext.Groups]);

			OpenPopupCommand = new RelayCommand(OpenPopup);
			SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
			ClosePopupCommand = new RelayCommand(ClosePopup);
			UpdateEntityCommand = new RelayCommand(UpdateEntity);
		}

		public override void OpenPopup(object obj)
		{
			try
			{
				if (obj is null || obj is not Group objAsGroup)
					GroupData = new();
				else
					GroupData = objAsGroup;

				IsPopupOpen = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public override bool CanSaveChanges(object obj)
		{
			return Validator.TryValidateObject(this, new ValidationContext(this), null) && !HasErrors;
		}

		public override void SaveChanges(object obj)
		{
			try
			{
                var objAsGroup = (obj as Group)!;

                if (!IsUpdate && GroupExists(Title))
				{
					Console.WriteLine($"The group with \"{Title}\" title is already available");
					return;
				}

				if (objAsGroup.Id == 0)
				{
					var group = new Group()
					{
						Title = Title
					};

					_dbContext.Groups.Add(group);
				}
				else
				{
					var group = _dbContext.Groups.FirstOrDefault(g => g.Id == objAsGroup.Id)!;

					group.Title = Title;

					_dbContext.Entry(group).State = EntityState.Modified;
				}

				_dbContext.SaveChanges();
				Groups = new ObservableCollection<Group>([.. _dbContext.Groups]);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				ClosePopup(obj);
			}
		}

		public override void ClosePopup(object obj)
		{
			IsPopupOpen = false;

			Title = string.Empty;

            IsUpdate = false;
			ClearAllErrors();
        }

		public override void UpdateEntity(object obj)
		{
            var objAsGroup = (obj as Group)!;

            Title = objAsGroup.Title;

            IsUpdate = true;
            OpenPopup(obj);
		}

		public override void DeleteEntity(object obj)
		{
            var objAsGroup = (obj as Group)!;
            var group = _dbContext.Groups.FirstOrDefault(g => g.Id == objAsGroup.Id)!;

			_dbContext.Groups.Remove(group);
			_dbContext.SaveChanges();

			Groups = new ObservableCollection<Group>([.. _dbContext.Groups]);
		}

		private bool GroupExists(string? title)
		{
			return _dbContext.Groups.Any(g => g.Title == title);
		}

        public void SearchData(string pattern)
        {
            Groups = new ObservableCollection<Group>([.. _dbContext.Groups.Where(g => g.Title!.Contains(pattern.ToLower()))]);
        }
    }
}
