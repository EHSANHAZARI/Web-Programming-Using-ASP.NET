// **************************************************
// WEB524 Project Template V3 == 33cfa3b9-87dd-403d-a5c3-0fc68c390aeb
// Do not change this header.
// **************************************************

using AutoMapper;
using F2021A6MH.EntityModels;
using F2021A6MH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Data.Entity;

namespace F2021A6MH.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                // Object mapper definitions

                // Genre
                cfg.CreateMap<Genre, GenreBaseViewModel>();

                // Album
                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<AlbumBaseViewModel, AlbumAddFormViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();

                // Artis
                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistBaseViewModel, ArtistAddFormViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();
                cfg.CreateMap<Artist, ArtistWithMediaInfoViewModel>();

                // Track
                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<TrackBaseViewModel, TrackAddFormViewModel>();
                cfg.CreateMap<Track, TrackAudioViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<TrackWithDetailViewModel, TrackAddFormViewModel>();
                cfg.CreateMap<TrackWithDetailViewModel, TrackEditFormViewModel>();

                // Artist Media Item
                cfg.CreateMap<ArtistMediaItem, ArtistMediaItemBaseViewModel>();
                cfg.CreateMap<ArtistMediaItem, ArtistMediaContentViewModel>();
                cfg.CreateMap<ArtistMediaItemAddViewModel, ArtistMediaItem>();
                cfg.CreateMap<ArtistMediaItemBaseViewModel, ArtistMediaItemAddFormViewModel>();

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // ############################################################
        // RoleClaim

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()

        #region Genre
        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var genres = ds.Genres.OrderBy(c => c.Name);

            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(genres);
        }
        #endregion

        #region Artist
        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            var artists = ds.Artists.OrderBy(c => c.Name);

            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(artists);
        }

        public ArtistWithDetailViewModel ArtistGetById(int? id)
        {
            var artist = ds.Artists.Include(c => c.Albums).SingleOrDefault(c => c.Id == id);

            if (artist == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Artist, ArtistWithDetailViewModel>(artist);
                result.AlbumNames = artist.Albums.Select(r => r.Name);

                return result;
            }
        }

        public ArtistWithMediaInfoViewModel ArtistWithMediaInformationGetById(int? id)
        {
            var artist = ds.Artists
                .Include(c => c.Albums)
                .Include(c => c.ArtistMediaItems)
                .SingleOrDefault(c => c.Id == id);

            if (artist == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Artist, ArtistWithMediaInfoViewModel>(artist);
                result.AlbumNames = artist.Albums.Select(r => r.Name);

                return result;
            }
        }

        public ArtistWithDetailViewModel ArtistAdd(ArtistAddViewModel newArtist)
        {
            newArtist.Executive = HttpContext.Current.User.Identity.Name;

            var addedArtist = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newArtist));

            ds.SaveChanges();

            return addedArtist == null ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(addedArtist);
        }

        public ArtistWithMediaInfoViewModel ArtistMediaAdd(ArtistMediaItemAddViewModel newMedia)
        {

            var artist = ds.Artists.Find(newMedia.ArtistId);

            if (artist == null)
            {
                return null;
            }
            else
            {
                var addedMedia = new ArtistMediaItem();

                ds.ArtistMediaItems.Add(addedMedia);

                addedMedia.Caption = newMedia.Caption;
                addedMedia.Artist = artist;

                byte[] mediaBytes = new byte[newMedia.MediaUpload.ContentLength];
                newMedia.MediaUpload.InputStream.Read(mediaBytes, 0, newMedia.MediaUpload.ContentLength);

                addedMedia.Content = mediaBytes;
                addedMedia.ContentType = newMedia.MediaUpload.ContentType;

                ds.SaveChanges();

                return (addedMedia == null) ? null : mapper.Map<Artist, ArtistWithMediaInfoViewModel>(artist);
            }
        }

        public ArtistMediaContentViewModel MediaGetById(string stringId)
        {
            var media = ds.ArtistMediaItems.SingleOrDefault(mi => mi.StringId == stringId);

            return (media == null) ? null : mapper.Map<ArtistMediaItem, ArtistMediaContentViewModel>(media);
        }

        #endregion

        #region Album
        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            var albums = ds.Albums.OrderBy(c => c.ReleaseDate);

            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(albums);
        }

        public AlbumWithDetailViewModel AlbumGetById(int? id)
        {
            var album = ds.Albums
                .Include(c => c.Artists)
                .Include(c => c.Tracks)
                .SingleOrDefault(c => c.Id == id);

            if (album == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Album, AlbumWithDetailViewModel>(album);
                result.ArtistNames = album.Artists.Select(a => a.Name);
                return result;
            }
        }

        public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel newAlbum)
        {
            newAlbum.Coordinator = HttpContext.Current.User.Identity.Name;

            var addedAlbum = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newAlbum));

            ds.SaveChanges();

            return addedAlbum == null ? null : mapper.Map<Album, AlbumWithDetailViewModel>(addedAlbum);
        }
        #endregion

        #region Track
        public IEnumerable<TrackBaseViewModel> TrackGetAll()
        {
            var tracks = ds.Tracks.OrderBy(c => c.Name);

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(tracks);
        }

        public IEnumerable<TrackBaseViewModel> TrackGetAllByArtistId(int? artistId)
        {
            var artist = ds.Artists
                .Include("Albums.Tracks")
                .SingleOrDefault(a => a.Id == artistId);

            if (artist == null)
            {
                return null;
            }

            var tracks = artist.Albums.SelectMany(c => c.Tracks);

            tracks = tracks.Distinct().ToList();

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(tracks.OrderBy(c => c.Name));
        }

        public TrackWithDetailViewModel TrackGetById(int? id)
        {
            var track = ds.Tracks.Include("Albums.Artists").SingleOrDefault(t => t.Id == id);

            if (track == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Track, TrackWithDetailViewModel>(track);
                result.AlbumNames = track.Albums.Select(a => a.Name);
                return result;
            }
        }

        public TrackBaseViewModel TrackAdd(TrackAddViewModel newTrack)
        {
            var album = ds.Albums.Find(newTrack.AlbumId);

            if (album != null)
            {
                newTrack.Albums = new List<Album> { album };
            }

            newTrack.Clerk = HttpContext.Current.User.Identity.Name;
            var addedTrack = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newTrack));

            byte[] audioBytes = new byte[newTrack.AudioUpload.ContentLength];
            newTrack.AudioUpload.InputStream.Read(audioBytes, 0, newTrack.AudioUpload.ContentLength);
            addedTrack.Audio = audioBytes;
            addedTrack.AudioContentType = newTrack.AudioUpload.ContentType;

            ds.SaveChanges();

            return (addedTrack != null) ? mapper.Map<Track, TrackWithDetailViewModel>(addedTrack) : null;
        }

        public TrackAudioViewModel TrackAudioGetById(int id)
        {
            var track = ds.Tracks.Find(id);

            return (track == null) ? null : mapper.Map<Track, TrackAudioViewModel>(track);
        }

        public bool TrackDelete(int? id)
        {
            var track = ds.Tracks.Find(id);

            if (track == null)
            {
                return false;
            }
            else
            {
                ds.Tracks.Remove(track);
                ds.SaveChanges();

                return true;
            }
        }

        public TrackWithDetailViewModel TrackEdit(TrackEditViewModel editedTrack)
        {
            var track = ds.Tracks.Find(editedTrack.Id);

            if (track == null)
            {
                return null;
            }
            else
            {

                byte[] audioBytes = new byte[editedTrack.AudioUpload.ContentLength];
                editedTrack.AudioUpload.InputStream.Read(audioBytes, 0, editedTrack.AudioUpload.ContentLength);

                track.Audio = audioBytes;
                track.AudioContentType = editedTrack.AudioUpload.ContentType;

                ds.SaveChanges();

                return mapper.Map<Track, TrackWithDetailViewModel>(track);
            }
        }
        #endregion


        // Add some programmatically-generated objects to the data store
        // Can write one method, or many methods - your decision
        // The important idea is that you check for existing data first
        // Call this method from a controller action/method
        public bool InitialGenreData()
        {
            if (ds.Genres.Count() > 0) return false;

            ds.Genres.Add(new Genre { Name = "Rap Rock" });
            ds.Genres.Add(new Genre { Name = "Alternative Rock" });
            ds.Genres.Add(new Genre { Name = "Hard Rock" });
            ds.Genres.Add(new Genre { Name = "Pop" });
            ds.Genres.Add(new Genre { Name = "Jazz" });
            ds.Genres.Add(new Genre { Name = "R&B" });
            ds.Genres.Add(new Genre { Name = "House" });
            ds.Genres.Add(new Genre { Name = "Rap" });
            ds.Genres.Add(new Genre { Name = "Disco" });
            ds.Genres.Add(new Genre { Name = "Classic" });

            ds.SaveChanges();

            return true;
        }

        public bool InitialArtistData()
        {
            if (ds.Artists.Count() > 0) return false;

            var exec = HttpContext.Current.User.Identity.Name;

            ds.Artists.Add(new Artist
            {
                Name = "Linkin Park",
                UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/LPLogo-black.svg/563px-LPLogo-black.svg.png",
                BirthName = "Linkin Park",
                BirthOrStartDate = new DateTime(1996, 1, 1),
                Executive = exec,
                Genre = "Rap Rock",
            });

            ds.Artists.Add(new Artist
            {
                Name = "Peots of the Fall (POTF)",
                UrlArtist = "https://i.pinimg.com/originals/6b/df/5f/6bdf5f83bb867eb74bbcce9788b1cce5.jpg",
                BirthName = "Peots of the Fall (POTF)",
                BirthOrStartDate = new DateTime(2003, 6, 25),
                Executive = exec,
                Genre = "Alternative Rock",
            });

            ds.Artists.Add(new Artist
            {
                Name = "Steven Tyler",
                UrlArtist = "https://cdns-images.dzcdn.net/images/artist/f8c9d6420136903f17dd588e8d414c0e/264x264.jpg",
                BirthName = "Steven Victor Tallarico",
                BirthOrStartDate = new DateTime(1948, 4, 26),
                Executive = exec,
                Genre = "Hard Rock",
            });

            ds.SaveChanges();

            return true;
        }

        public bool InitialAlbumData()
        {
            if (ds.Albums.Count() > 0) return false;

            var coord = HttpContext.Current.User.Identity.Name;

            var genre = ds.Genres.SingleOrDefault(g => g.Name == "Rap Rock");
            if (genre == null) { return false; }

            var artist = ds.Artists.SingleOrDefault(a => a.Name == "Linkin Park");
            if (artist == null) { return false; }

            ds.Albums.Add(new Album
            {
                Name = "Hybrid Theory",
                Genre = genre.Name,
                UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/2/2a/Linkin_Park_Hybrid_Theory_Album_Cover.jpg/220px-Linkin_Park_Hybrid_Theory_Album_Cover.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2000, 10, 24),
                Artists = new List<Artist> { artist }
            });

            ds.Albums.Add(new Album
            {
                Name = "Meteora",
                Genre = genre.Name,
                UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/0/0c/Linkin_Park_Meteora_Album_Cover.jpg/220px-Linkin_Park_Meteora_Album_Cover.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2003, 3, 25),
                Artists = new List<Artist> { artist }
            });

            ds.SaveChanges();

            return true;
        }

        public bool InitialTrackData()
        {
            if (ds.Tracks.Count() > 0) return false;

            var clerk = HttpContext.Current.User.Identity.Name;
            var genre = ds.Genres.SingleOrDefault(g => g.Name == "Rap Rock");

            var hyrbridTheory = ds.Albums.SingleOrDefault(a => a.Name == "Hybrid Theory");
            if (hyrbridTheory != null)
            {
                ds.Tracks.Add(new Track
                {
                    Name = "Papercut",
                    Clerk = clerk,
                    Composers = "Linkin Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "One Step Closer",
                    Clerk = clerk,
                    Composers = "Linkin Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "With You",
                    Clerk = clerk,
                    Composers = "Linkin Park, Michael Simpson, John King",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Points of Authority",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Crawling",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Runaway",
                    Clerk = clerk,
                    Composers = "Linkin Park, Mark Wakefield",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "By Myself",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "In the End",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { hyrbridTheory }
                });
            }


            var meteora = ds.Albums.SingleOrDefault(a => a.Name == "Meteora");
            if (meteora != null)
            {

                ds.Tracks.Add(new Track
                {
                    Name = "Foreword",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Don't Stay",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Lying from You",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Hit the Floor",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Easier to Run",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Faint",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Figure.09",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });

                ds.Tracks.Add(new Track
                {
                    Name = "Breaking the Habit",
                    Clerk = clerk,
                    Composers = "Linking Park",
                    Genre = genre.Name,
                    Albums = new List<Album> { meteora }
                });
            }

            ds.SaveChanges();

            return true;
        }

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // Role claims

            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                ds.SaveChanges();
                done = true;
            }

            InitialGenreData();
            InitialArtistData();
            InitialAlbumData();
            InitialTrackData();

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var e in ds.ArtistMediaItems)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                
                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    // New "RequestUser" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it

    // How to use...

    // In the Manager class, declare a new property named User
    //public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value
    //User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }
}