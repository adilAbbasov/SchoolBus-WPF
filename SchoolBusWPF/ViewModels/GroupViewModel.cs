using Microsoft.EntityFrameworkCore;
using SCHOOL_BUS.Commands;
using SchoolBusWPF.Models.Concretes;
using System.Collections.ObjectModel;

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
			DeleteEntityCommand = new RelayCommand(DeleteEntity);
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
			return !HasErrors;
        }

		public override void SaveChanges(object obj)
		{
			try
			{
                if (obj is not Group objAsGroup)
                    return;

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
					var group = _dbContext.Groups.FirstOrDefault(g => g.Id == objAsGroup.Id);

					if (group is null)
						return;

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
			if (obj is null || obj is not Group objAsGroup)
				return;

			Title = objAsGroup.Title;

            IsUpdate = true;
            OpenPopup(obj);
		}

		public override void DeleteEntity(object obj)
		{
			if (obj is null || obj is not Group objAsGroup)
				return;

			var group = _dbContext.Groups.FirstOrDefault(g => g.Id == objAsGroup.Id);

			if (group is null)
				return;

			_dbContext.Groups.Remove(group);
			_dbContext.SaveChanges();
			Groups = new ObservableCollection<Group>([.. _dbContext.Groups]);
		}

		private bool GroupExists(string? title)
		{
			return _dbContext.Groups.Any(g => g.Title == title);
		}
	}
}
